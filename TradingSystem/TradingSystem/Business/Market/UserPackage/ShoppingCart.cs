using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ShoppingCart :IShoppingCart
    {
        private SortedDictionary<IStore, IShoppingBasket> _store_shoppingBasket;

        public SortedDictionary<IStore, IShoppingBasket> Store_shoppingBasket { get => _store_shoppingBasket; set => _store_shoppingBasket = value; }

        public ShoppingCart()
        {
            this.Store_shoppingBasket = new SortedDictionary<IStore, IShoppingBasket>();
        }

        public bool IsEmpty()
        {
            var notEmptyBaskets = Store_shoppingBasket.Values.Where(shoppingBasket => !shoppingBasket.IsEmpty());
            return !notEmptyBaskets.Any();
        }
        public IShoppingBasket GetShoppingBasket(IStore store)
        {
            //create if not exists
            if (!Store_shoppingBasket.ContainsKey(store))
            {
                Store_shoppingBasket.Add(store, new ShoppingBasket());
            }
            return Store_shoppingBasket[store];
        }

        public void AddShoppingBasket(IStore store, IShoppingBasket shoppingBasket)
        {
            Store_shoppingBasket.Add(store, shoppingBasket);
        }

        public void RemoveShoppingBasket(IStore store)
        {
            Store_shoppingBasket.Remove(store);
        }

        public void UpdateShoppingBasket(IStore store, IShoppingBasket shoppingBasket)
        {
            //create if not exists
            if (!Store_shoppingBasket.Keys.Contains(store))
            {
                Store_shoppingBasket.Add(store, shoppingBasket);
            }
            Store_shoppingBasket[store] = shoppingBasket;

        }

        public bool CheckPolicy()
        {
            bool isLegal = true;
            foreach (KeyValuePair<IStore, IShoppingBasket> s_sb in Store_shoppingBasket)
            {
                isLegal = isLegal && s_sb.Key.CheckPolicy(s_sb.Value);
            }
            return isLegal;
        }

        public BuyStatus Purchase(string username, PaymentMethod method, string clientPhone, Address clientAddress, double paySum)
        {
            ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
            foreach (KeyValuePair<IStore, IShoppingBasket> s_sb in Store_shoppingBasket)
            {
                PurchaseStatus purchaseStatus = s_sb.Key.Purchase(s_sb.Value, username, clientPhone, clientAddress, method, paySum);
                purchases.Add(purchaseStatus);
            }
            //If failed to make all the trasactions, need to cancel deliveries and payments
            IEnumerable<PurchaseStatus> failedPayments = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.PaymentStatus != null && pur.TransactionStatus.PaymentStatus.Status == false);
            IEnumerable<PurchaseStatus> failedDeliveries = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.DeliveryStatus != null && pur.TransactionStatus.DeliveryStatus.Status == false);
            CancelPurchase(failedPayments, cancelPayments: true);
            CancelPurchase(failedDeliveries, cancelDeliveries: true);
            //means no transactions failed
            bool allSuceeded = !failedDeliveries.Any() && !failedPayments.Any();
            return new BuyStatus(allSuceeded, purchases);
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
            return Store_shoppingBasket.Aggregate(0.0, (total, next) => total + next.Key.CalcPaySum(next.Value));
        }

        public IDictionary<Guid, IDictionary<Guid , int >> GetShopingCartProducts()
        {
            IDictionary<Guid, IDictionary<Guid, int>> ret = new Dictionary<Guid, IDictionary<Guid, int>>();
            foreach (IStore store in _store_shoppingBasket.Keys)
            {
                IDictionary<Product, int> dict = _store_shoppingBasket[store].GetDictionaryProductQuantity();
                IDictionary<Guid, int> dictToAdd = FilterDictionary(dict);
                ret.Add(store.GetId(), dictToAdd);
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
