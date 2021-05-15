using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    public class ShoppingCart :IShoppingCart
    {
        private SortedSet<IShoppingBasket> shoppingBaskets;

        private User _user;

        public SortedSet<IShoppingBasket> ShoppingBaskets { get => shoppingBaskets; set => shoppingBaskets = value; }
        public User User1 { get => _user; set => _user = value; }

        public ShoppingCart(User user)
        {
            this.shoppingBaskets = new SortedSet<IShoppingBasket>();
            _user = user;
        }

        public ShoppingCart(ShoppingCart c)
        {
            _user = c.User1;
            this.shoppingBaskets = new SortedSet<IShoppingBasket>();
            foreach(IShoppingBasket s in c.ShoppingBaskets)
            {
                this.shoppingBaskets.Add(new ShoppingBasket((ShoppingBasket)s, this));
            }
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

        public void removeBasket(IStore store)
        {
            if (shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).Any())
            {
                shoppingBaskets.Remove(shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).FirstOrDefault());
            }
        }

        public IShoppingBasket TryGetShoppingBasket(IStore store)
        {
            //create if not exists
            if (!shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).Any())
            {
                return null;
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

        public async Task<BuyStatus> Purchase(PaymentMethod method, string clientPhone, Address clientAddress)
        {
            //chcek is not empty
            if (IsEmpty()) return new BuyStatus(false, null);
            bool allStatusesOk = true;
            ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
            foreach (IShoppingBasket basket in shoppingBaskets)
            {
                PurchaseStatus purchaseStatus = await basket.GetStore().Purchase(basket, clientPhone, clientAddress, method);
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

        public User GetUser()
        {
            return _user;
        }
    }
}
