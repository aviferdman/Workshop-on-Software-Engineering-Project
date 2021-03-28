using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    class Store
    {
        private ICollection<Product> _products;
        private ICollection<TransactionStatus> _transactionsHistory;
        private Guid _id;
        private Discount _discount;
        private Policy _policy;
        private Address _address;
        private static Transaction _transaction = Transaction.Instance;
        private object _lock;

        internal ICollection<Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        internal ICollection<TransactionStatus> TransactionsHistory { get => _transactionsHistory; set => _transactionsHistory = value; }
        public Guid Id { get => _id; set => _id = value; }

        public Store()
        {
            this._products = new HashSet<Product>();
            this._transactionsHistory = new HashSet<TransactionStatus>();
            this._lock = new object();
            this._discount = new Discount();
            this._id = new Guid();
            this._policy = new Policy();
        }

        public PurchaseStatus Purchase(Dictionary<Product, int> product_quantity, Guid clientId, string clientPhone, Address clientAddress, Guid clientBankAccount, double paymentSum)
        {
            bool enoughtQuantity, enoughtCurrent;
            TransactionStatus transactionStatus;
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            enoughtCurrent = CheckEnoughtCurrent(clientBankAccount, paymentSum);
            //pre-conditions not legal
            if (!enoughtCurrent) return new PurchaseStatus(false, null, _id, product_quantity);

            lock (_lock)
            {
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, _id, product_quantity);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = _transaction.ActivateTransaction(clientId, clientPhone, weight, _address, clientAddress, clientBankAccount, _id, getBankAccount(), paymentSum);
            _transactionsHistory.Add(transactionStatus);
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            return new PurchaseStatus(true, transactionStatus, _id, product_quantity);
        }

        private bool CheckEnoughtCurrent(Guid clientBankAccount, double paymentSum)
        {
            throw new NotImplementedException();
        }

        public void CancelTransaction(Dictionary<Product, int> product_quantity)
        {
            UpdateQuantities(product_quantity, false);
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public double CalcPaySum(ShoppingBasket shoppingBasket)
        {
            double discount = _discount.ApplyDiscounts(shoppingBasket);
            double cost = shoppingBasket.CalcCost();
            return cost - discount;
        }

        //use case 12 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/75
        public bool CheckPolicy(ShoppingBasket shoppingBasket)
        {
            return Policy.Check(shoppingBasket.Product_quantity);
        }

        private Guid getBankAccount()
        {
            throw new NotImplementedException();
        }

        private void UpdateQuantities(Dictionary<Product, int> product_quantity, bool subOrAdd = true)
        {
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                Product product = getProductById(p_q.Key.Id);
                if (subOrAdd)
                {   
                    product.Quantity = product.Quantity - p_q.Value;
                }
                else
                {
                    product.Quantity = product.Quantity + p_q.Value;
                }
            }
        }

        private bool EnoughQuantity(Dictionary<Product, int> product_quantity)
        {
            bool enoughtQuantity = true;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                Guid productId = p_q.Key.Id;
                Product p = getProductById(productId);
                enoughtQuantity = enoughtQuantity && p != null && p.Quantity >= p_q.Value;
            }
            return enoughtQuantity;
        }

        private Product getProductById(Guid productId)
        {
            IEnumerable<Product> products = Products.Where(product => product.Id.Equals(productId));
            return products.FirstOrDefault();
        }

    }
}
