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
        private ICollection<User> _users;
        private ICollection<ShoppingCart> _shoppingCarts;
        private ICollection<ShoppingBasket> _shoppingBaskets;
        private object _transactionLock;
        private Address address;
        private static Transaction _transaction = Transaction.Instance;

        public Store()
        {
            this._products = new HashSet<Product>();
            this._users = new HashSet<User>();
            this._shoppingCarts = new HashSet<ShoppingCart>();
            this._shoppingBaskets = new HashSet<ShoppingBasket>();
            this._transactionLock = new object();
        }

        public bool Purchase(Dictionary<Product, int> product_quantity, string clientId, double paymentSum)
        {
            bool enoughtQuantity, enoughtCurrent, purvhaseStatus;
            User client = getUserById(clientId);
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight);

            lock (_transactionLock)
            {
                enoughtQuantity = EnoughQuantity(product_quantity);
                if (!enoughtQuantity) return false;

                enoughtCurrent = client.CheckEnoughtCurrent(paymentSum);
                if (!enoughtCurrent) return false;

                UpdateQuantities(product_quantity);

                purvhaseStatus = _transaction.ActivateTransaction(clientId, client.getPhoneNumber(), weight, address.ToString(), client.Address.ToString(), client.getBankAccount(), getBankAccount(), paymentSum);
                if (!purvhaseStatus)
                {
                    //transaction didn't succeded means no need to substract the quantities
                    UpdateQuantities(product_quantity, subOrAdd: false);
                }
            }

            return purvhaseStatus;
        }

        private string getBankAccount()
        {
            throw new NotImplementedException();
        }

        private void UpdateQuantities(IEnumerable<KeyValuePair<Product, int>> product_quantity, bool subOrAdd = true)
        {
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                if (subOrAdd)
                {
                    p_q.Key.Quantity = p_q.Key.Quantity - p_q.Value;
                }
                else
                {
                    p_q.Key.Quantity = p_q.Key.Quantity + p_q.Value;
                }
            }
        }

        private bool EnoughQuantity(IEnumerable<KeyValuePair<Product, int>> product_quantity)
        {
            bool enoughtQuantity = true;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                string productId = p_q.Key.Id;
                Product p = getProductById(productId);
                enoughtQuantity = enoughtQuantity && p != null && p.Quantity >= p_q.Value;
            }
            return enoughtQuantity;
        }

        private Product getProductById(string productId)
        {
            IEnumerable<Product> products = _products.Where(product => product.Id.Equals(productId));
            return products.FirstOrDefault();
        }

        private User getUserById(string clientId)
        {
            IEnumerable<User> users = _users.Where(user => user.Id.Equals(clientId));
            return users.FirstOrDefault();
        }
    }
}
