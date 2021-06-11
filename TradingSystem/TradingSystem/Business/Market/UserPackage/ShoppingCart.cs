using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market
{
    public class ShoppingCart 
    {
        public SortedSet<ShoppingBasket> shoppingBaskets { get; set; }
        public string username { get; set; }
        [NotMapped]
        private User _user;

        public SortedSet<ShoppingBasket> ShoppingBaskets { get => shoppingBaskets; set => shoppingBaskets = value; }
        [NotMapped]
        public User User1 { get => _user; set => _user = value; }

        public ShoppingCart(User user)
        {
            this.shoppingBaskets = new SortedSet<ShoppingBasket>();
            _user = user;
            username = user.Username;
        }

        public ShoppingCart(string username)
        {
            this.shoppingBaskets = new SortedSet<ShoppingBasket>();
            this.username = username;
        }
        public ShoppingCart()
        {
            this.shoppingBaskets = new SortedSet<ShoppingBasket>();
        }

        public ShoppingCart(ShoppingCart c)
        {
            _user = c.User1;
            this.shoppingBaskets = new SortedSet<ShoppingBasket>();
            foreach(ShoppingBasket s in c.ShoppingBaskets)
            {
                this.shoppingBaskets.Add(new ShoppingBasket((ShoppingBasket)s, this));
            }
        }

        public bool IsEmpty()
        {
            var notEmptyBaskets = shoppingBaskets.Where(shoppingBasket => !shoppingBasket.IsEmpty());
            return !notEmptyBaskets.Any();
        }
        public async virtual Task<ShoppingBasket> GetShoppingBasket(Store store)
        {
            //create if not exists
            if (!shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).Any())
            {
                shoppingBaskets.Add(new ShoppingBasket(this, store));
                await ProxyMarketContext.Instance.saveChanges();
            }
            return shoppingBaskets.Where(basket => basket.GetStore().GetId().Equals(store.GetId())).First();
        }


        public virtual ShoppingBasket TryGetShoppingBasket(Store store)
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
            foreach (ShoppingBasket basket in shoppingBaskets)
            {
                isLegal = isLegal && basket.GetStore().CheckPolicy(basket);
            }
            return isLegal;
        }

        public async Task<BuyStatus> Purchase(PaymentMethod method, string clientPhone, Address clientAddress)
        {
            IDbContextTransaction transaction = null;
            if (!ProxyMarketContext.Instance.IsDebug)
            {
                MarketUsers.Instance.s.WaitOne();
                transaction = MarketContext.Instance.Database.BeginTransaction();
            }
            try
            {
                //chcek is not empty
                if (IsEmpty()) 
                {
                    if (!ProxyMarketContext.Instance.IsDebug)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        MarketUsers.Instance.s.Release();
                    }
                        
                    return new BuyStatus(false, null); 
                }
                bool allStatusesOk = true;
                ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
                foreach (ShoppingBasket basket in shoppingBaskets)
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
                await ProxyMarketContext.Instance.saveChanges();
                if (!ProxyMarketContext.Instance.IsDebug)
                {
                    transaction.Commit();
                    transaction.Dispose();

                    MarketUsers.Instance.s.Release();
                }
                    
                return new BuyStatus(allSuceeded && allStatusesOk, purchases);
                
            }
            catch (Exception ex)
            {
                if (!ProxyMarketContext.Instance.IsDebug)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    MarketUsers.Instance.s.Release();
                }
                return new BuyStatus(true, null);
            }
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
            foreach (ShoppingBasket basket in shoppingBaskets)
            {
                HashSet<ProductInCart> dict = basket.GetDictionaryProductQuantity();
                IDictionary<Guid, int> dictToAdd = new Dictionary<Guid, int>();
                foreach(ProductInCart p in basket.Product_quantity)
                {
                    dictToAdd.Add(p.product.Id, p.quantity);
                }
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
