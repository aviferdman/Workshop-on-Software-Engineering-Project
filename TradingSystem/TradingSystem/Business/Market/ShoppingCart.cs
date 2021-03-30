using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ShoppingCart
    {
        private SortedDictionary<Store, ShoppingBasket> _store_shoppingBasket;

        public ShoppingCart()
        {
            this._store_shoppingBasket = new SortedDictionary<Store, ShoppingBasket>();
        }

        public bool IsEmpty()
        {
            var notEmptyBaskets = _store_shoppingBasket.Values.Where(shoppingBasket => !shoppingBasket.IsEmpty());
            return !notEmptyBaskets.Any();
        }
        public ShoppingBasket GetShoppingBasket(Store store)
        {
            //create if not exists
            if (!_store_shoppingBasket.ContainsKey(store))
            {
                _store_shoppingBasket.Add(store, new ShoppingBasket());
            }
            return _store_shoppingBasket[store];
        }

        public void AddShoppingBasket(Store store, ShoppingBasket shoppingBasket)
        {
            _store_shoppingBasket.Add(store, shoppingBasket);
        }

        public void RemoveShoppingBasket(Store store)
        {
            _store_shoppingBasket.Remove(store);
        }

        public void UpdateShoppingBasket(Store store, ShoppingBasket shoppingBasket)
        {
            if (_store_shoppingBasket[store] == null)
            {
                _store_shoppingBasket.Add(store, null);
            }
            _store_shoppingBasket[store] = shoppingBasket;

        }

        public bool CheckPolicy()
        {
            bool isLegal = true;
            foreach (KeyValuePair<Store, ShoppingBasket> s_sb in _store_shoppingBasket)
            {
                isLegal = isLegal && s_sb.Key.CheckPolicy(s_sb.Value);
            }
            return isLegal;
        }

        public bool Purchase(Guid clientId, BankAccount clientBankAccount, string clientPhone, Address clientAddress, double paySum)
        {
            ICollection<PurchaseStatus> purchases = new HashSet<PurchaseStatus>();
            foreach (KeyValuePair<Store, ShoppingBasket> s_sb in _store_shoppingBasket)
            {
                PurchaseStatus purchaseStatus = s_sb.Key.Purchase(s_sb.Value.Product_quantity, clientId, clientPhone, clientAddress, clientBankAccount, paySum);
                purchases.Add(purchaseStatus);
            }
            //If failed to make all the trasactions, need to cancel deliveries and payments
            IEnumerable<PurchaseStatus> failedPayments = purchases.Where(pur => pur.PreConditions == true && pur.TransactionStatus.PaymentStatus != null && pur.TransactionStatus.PaymentStatus.Status == false);
            IEnumerable<PurchaseStatus> failedDeliveries = purchases.Where(pur => pur.PreConditions == true && pur.TransactionStatus.DeliveryStatus != null && pur.TransactionStatus.DeliveryStatus.Status == false);
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
            return _store_shoppingBasket.Aggregate(0.0, (total, next) => total + next.Key.CalcPaySum(next.Value));
        }
    }
}
