using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ShoppingCart :IShoppingCart
    {
        private SortedSet<IShoppingBasket> shoppingBaskets;

        public SortedSet<IShoppingBasket> ShoppingBaskets { get => shoppingBaskets; set => shoppingBaskets = value; }

        public ShoppingCart()
        {
            this.shoppingBaskets = new SortedSet<IShoppingBasket>();
        }

        public bool IsEmpty()
        {
            var notEmptyBaskets = shoppingBaskets.Where(shoppingBasket => !shoppingBasket.IsEmpty());
            return !notEmptyBaskets.Any();
        }
        public IShoppingBasket GetShoppingBasket(IStore store)
        {
            //create if not exists
            if (!shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).Any())
            {
                shoppingBaskets.Add(new ShoppingBasket(this, store));
            }
            return shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).FirstOrDefault();
        }

        public bool CheckPolicy()
        {
            bool isLegal = true;
            foreach (IShoppingBasket basket in shoppingBaskets)
            {
                isLegal = isLegal && basket.GetStore().CheckPolicy(basket);
            }
            return isLegal;
        }

        public BuyStatus Purchase(string username, PaymentMethod method, string clientPhone, Address clientAddress, double paySum)
        {
            bool allStatusesOk = true;
            ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
            foreach (IShoppingBasket basket in shoppingBaskets)
            {
                PurchaseStatus purchaseStatus = basket.GetStore().Purchase(basket, username, clientPhone, clientAddress, method, paySum);
                purchases.Add(purchaseStatus);
                allStatusesOk = allStatusesOk && purchaseStatus.TransactionStatus != null && purchaseStatus.TransactionStatus.Status;
            }
            //If failed to make all the trasactions, need to cancel deliveries and payments
            IEnumerable<PurchaseStatus> failedPayments = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.PaymentStatus != null && pur.TransactionStatus.PaymentStatus.Status == false);
            IEnumerable<PurchaseStatus> failedDeliveries = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.DeliveryStatus != null && pur.TransactionStatus.DeliveryStatus.Status == false);
            CancelPurchase(failedPayments, cancelPayments: true);
            CancelPurchase(failedDeliveries, cancelDeliveries: true);
            //means no transactions failed
            bool allSuceeded = !failedDeliveries.Any() && !failedPayments.Any();
            return new BuyStatus(allSuceeded && allStatusesOk, purchases);
        }

        private void CancelPurchase(IEnumerable<PurchaseStatus> purchases, bool cancelDeliveries = false, bool cancelPayments = false)
        {
            //refund and cancel deliveries
            foreach (PurchaseStatus purcahse in purchases)
            {
                Transaction.Instance.CancelTransaction(purcahse.TransactionStatus, cancelPayments, cancelDeliveries);
            }
        }

        public double CalcPaySum()
        {
            return shoppingBaskets.Aggregate(0.0, (total, next) => total + next.GetStore().CalcPaySum(next));
        }

        public IDictionary<Guid, IDictionary<Guid , int >> GetShopingCartProducts()
        {
            IDictionary<Guid, IDictionary<Guid, int>> ret = new Dictionary<Guid, IDictionary<Guid, int>>();
            foreach (IShoppingBasket basket in shoppingBaskets)
            {
                IDictionary<Product, int> dict = basket.GetDictionaryProductQuantity();
                IDictionary<Guid, int> dictToAdd = FilterDictionary(dict);
                ret.Add(basket.GetStore().GetId(), dictToAdd);
            }

            return ret;
        }

        private IDictionary<Guid, int> FilterDictionary(IDictionary<Product, int> dict)
        {
            IDictionary<Guid, int> ret = new Dictionary<Guid, int>();
            foreach (Product p in dict.Keys)
            {
                ret.Add(p.Id, dict[p]);
            }
            return ret;
        }
    }
}
