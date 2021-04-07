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

        public bool Purchase(Guid clientId, BankAccount clientBankAccount, string clientPhone, Address clientAddress, double paySum)
        {
            ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
            foreach (KeyValuePair<IStore, IShoppingBasket> s_sb in Store_shoppingBasket)
            {
                PurchaseStatus purchaseStatus = s_sb.Key.Purchase(s_sb.Value.GetDictionaryProductQuantity(), clientId, clientPhone, clientAddress, clientBankAccount, paySum);
                purchases.Add(purchaseStatus);
            }
            //If failed to make all the trasactions, need to cancel deliveries and payments
            IEnumerable<PurchaseStatus> failedPayments = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.PaymentStatus != null && pur.TransactionStatus.PaymentStatus.Status == false);
            IEnumerable<PurchaseStatus> failedDeliveries = purchases.Where(pur => pur.GetPreConditions() == true && pur.TransactionStatus.DeliveryStatus != null && pur.TransactionStatus.DeliveryStatus.Status == false);
            CancelPurchase(failedPayments, cancelDeliveries: false, cancelPayments: true);
            CancelPurchase(failedDeliveries, cancelDeliveries: true, cancelPayments: false);
            bool allSuceeded = !failedDeliveries.Any() && !failedPayments.Any();
            //means no transactions failed
            return allSuceeded;
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
    }
}
