using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    public class Store : IStore
    {
        private ConcurrentDictionary<String, Product> _products;
        private ICollection<TransactionStatus> _transactionsHistory;
        private ConcurrentDictionary<Guid, StorePermission> personnel;
        private BankAccount _bank;
        private Guid _id;
        private string name;
        private ICollection<Discount> _discounts;
        private Policy _policy;
        private Address _address;
        private static Transaction _transaction = Transaction.Instance;
        private object _lock;

        public ConcurrentDictionary<String, Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        internal ICollection<TransactionStatus> TransactionsHistory { get => _transactionsHistory; set => _transactionsHistory = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => name; set => name = value; }
        internal BankAccount Bank { get => _bank; set => _bank = value; }
        public ICollection<Discount> Discounts { get => _discounts; set => _discounts = value; }
        public ConcurrentDictionary<Guid, StorePermission> Personnel { get => personnel; set => personnel = value; }

        public Store(string name, BankAccount bank, Address address)
        {
            this.name = name;
            this._products = new ConcurrentDictionary<string, Product>();
            this._transactionsHistory = new HashSet<TransactionStatus>();
            this._lock = new object();
            this.Discounts = new HashSet<Discount>();
            this._id = Guid.NewGuid();
            this._policy = new Policy();
            this._bank = bank;
            this._address = address;
            this.personnel = new ConcurrentDictionary<Guid, StorePermission>();
        }

        public Guid GetId()
        {
            return _id;
        }

        public PurchaseStatus Purchase(Dictionary<Product, int> product_quantity, Guid clientId, string clientPhone, Address clientAddress, BankAccount clientBankAccount, double paymentSum)
        {
            bool enoughtQuantity;
            TransactionStatus transactionStatus;
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);

            lock (_lock)
            {
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, _id, product_quantity);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = _transaction.ActivateTransaction(clientId, clientPhone, weight, _address, clientAddress, clientBankAccount, _id, _bank, paymentSum, product_quantity);
            _transactionsHistory.Add(transactionStatus);
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            return new PurchaseStatus(true, transactionStatus, _id, product_quantity);
        }

        public void CancelTransaction(Dictionary<Product, int> product_quantity)
        {
            UpdateQuantities(product_quantity, false);
        }

        public bool AddPerssonel(Guid username, Guid subjectUsername, AppointmentType permission)
        {
            StorePermission user;
            if (!personnel.TryGetValue(username, out user))
            {
                return false;
            }
            return personnel.TryAdd(subjectUsername,user.AddAppointment(subjectUsername, permission));
        }
        public bool RemovePerssonel(Guid username, Guid subjectUsername)
        {
            StorePermission user;
            StorePermission userToRem;
            if (!personnel.TryGetValue(username, out user))
            {
                return false;
            }
            if (!user.canRemoveAppointment(subjectUsername))
                return false;
            return personnel.TryRemove(subjectUsername, out userToRem);
        }
        public double CalcPaySum(IShoppingBasket shoppingBasket)
        {
            double discount = ApplyDiscounts(shoppingBasket);
            double cost = shoppingBasket.CalcCost();
            return cost - discount;
        }

        public double ApplyDiscounts(IShoppingBasket shoppingBasket)
        {
            var availableDiscounts = Discounts.Select(d=>d.ApplyDiscounts(shoppingBasket.GetDictionaryProductQuantity()));
            //chose the max value of an available discount
            try
            {
                return availableDiscounts.Max();
            }
            catch   // no discount is available
            {
                return 0;
            }
        }

        //use case 12 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/75
        public bool CheckPolicy(IShoppingBasket shoppingBasket)
        {
            return Policy.Check(shoppingBasket.GetDictionaryProductQuantity());
        }

        public void AddRule(IRule rule)
        {
            _policy.AddRule(rule);
        }

        public void RemoveRule(IRule rule)
        {
            _policy.RemoveRule(rule);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String AddProduct(Product product, Guid userID)
        {
            StorePermission permission;
            Personnel.TryGetValue(userID, out permission);
            if (permission == null)
                return "Invalid user";
            if (!permission.GetPermission(Permission.AddProduct))
                return "No permission";
            if (!validProduct(product))
                return "Invalid product";
            if (_products.ContainsKey(product.Name))
                return "Product exists";
            _products.TryAdd(product.Name, product);
            return "Product added";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(String productName, Guid userID)
        {
            StorePermission permission;
            Personnel.TryGetValue(userID, out permission);
            if (permission == null)
                return "Invalid user";
            if (!permission.GetPermission(Permission.RemoveProduct))
                return "No Permission";
            Product useless;
            _products.TryRemove(productName, out useless);
            return "Product removed";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(String productName, Product editedProduct, Guid userID)
        {
            StorePermission permission;
            Personnel.TryGetValue(userID, out permission);
            if (permission == null)
                return "Invalid user"; 
            if (!permission.GetPermission(Permission.EditProduct))
                return "No Permission";
            if (!validProduct(editedProduct))
                return "Invalid edit";
            
            Product oldProduct;
            _products.TryGetValue(productName, out oldProduct);
            if (oldProduct == null)
                return "Product not in the store";
            editedProduct.Id = oldProduct.Id;
            _products.TryRemove(productName, out oldProduct);
            _products.TryAdd(editedProduct.Name, editedProduct);
            return "Product edited";
        }

        public String AssignMember(Guid assigneeID, User assigner, AppointmentType type)
        {
            StorePermission assignee;
            StorePermission assignerPermission;
            if (personnel.TryGetValue(assigneeID, out assignee))
                return "this member is already assigned as a store owner or manager";
            if (!personnel.TryGetValue(assigner.Id, out assignerPermission))
                return "Invalid assigner";
            try
            {
                assignee = assignerPermission.AddAppointment(assigneeID, type);
            }
            catch { return "Invalid assigner"; }

            personnel.TryAdd(assigneeID, assignee);

            return "Success";
        }


        public void UpdateProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p!=null){    //remove old product if exists
                Product useless;
                _products.TryRemove(product.Name, out useless);
            }

            _products.TryAdd(product.Name, product); //add the new product

        }

        private bool validProduct(Product product)
        {
            if (product.Name == null || product.Name.Equals("") || product.Price < 0)
            {
                return false;
            }
            return true;
        }

        public void RemoveProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p != null)
            {    //remove old product if exists
                Product useless;
                _products.TryRemove(product.Name, out useless);
            }
        }

        public void AddDiscount(Discount discount)
        {
            Discounts.Add(discount);
        }

        public void RemoveDiscount(Guid discountId)
        {
            Discount d = GetDiscountById(discountId);
            if (d != null)
            {    //remove old discount if exists
                Discounts.Remove(d);
            }
        }


        private void UpdateQuantities(Dictionary<Product, int> product_quantity, bool subOrAdd = true)
        {
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                Product product = GetProductById(p_q.Key.Id);
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
                Product p = GetProductById(productId);
                enoughtQuantity = enoughtQuantity && p != null && p.Quantity >= p_q.Value;
            }
            return enoughtQuantity;
        }

        private Product GetProductById(Guid productId)
        {
            IEnumerable<Product> products = Products.Values.Where(product => product.Id.Equals(productId));
            return products.FirstOrDefault();
        }

        private Discount GetDiscountById(Guid discountId)
        {
            IEnumerable<Discount> discounts = Discounts.Where(discount => discount.Id.Equals(discountId));
            return discounts.FirstOrDefault();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public History GetStoreHistory(Guid userID)
        {
            bool isPermitted = CheckPermission(userID, Permission.GetShopHistory);
            if (isPermitted)
            {
                return _transaction.GetStoreHistory(_id);
            }
            throw new UnauthorizedAccessException();
        }

        private bool CheckPermission(Guid userId, Permission permission)
        {
            StorePermission storePermission;
            Personnel.TryGetValue(userId, out storePermission);
            return (storePermission != null && storePermission.GetPermission(permission));
        }

        public int CompareTo(object obj)
        {
            return obj.GetHashCode() - GetHashCode();
        }

        public ICollection<Product> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            List<Product> products = new List<Product>();
            foreach(Product p in _products.Values)
            {
                if(p.Name.Contains(keyword)|| p.Category.Contains(keyword))
                {
                    if (category != null&& !category.Equals(p.Category))
                        continue;
                    if (price_range_low != -1 && price_range_low < p.Price)
                        continue;
                    if (price_range_high != -1 && price_range_high > p.Price)
                        continue;
                    if (rating != -1 && rating != p.Rating)
                        continue;
                    products.Add(p);
                }
            }
            return products;
        }
    }
}
