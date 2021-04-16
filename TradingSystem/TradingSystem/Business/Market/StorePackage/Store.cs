using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class Store : IStore
    {
        private ConcurrentDictionary<String, Product> _products;
        private ICollection<TransactionStatus> _transactionsHistory;
        private Founder founder;
        private ConcurrentDictionary<string, Manager> managers;
        private ConcurrentDictionary<string, Owner> owners;
        private BankAccount _bank;
        private Guid _id;
        private string name;
        private ICollection<Discount> _discounts;
        private Policy _policy;
        private Address _address;
        private static Transaction _transaction = Transaction.Instance;
        private ICollection<IHistory> history;
        private object _lock;
        private object prem_lock;

        public ConcurrentDictionary<String, Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        internal ICollection<TransactionStatus> TransactionsHistory { get => _transactionsHistory; set => _transactionsHistory = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => name; set => name = value; }
        internal BankAccount Bank { get => _bank; set => _bank = value; }
        public ICollection<Discount> Discounts { get => _discounts; set => _discounts = value; }
        public ICollection<IHistory> History { get => history; set => history = value; }
        public ConcurrentDictionary<string, Manager> Managers { get => managers; set => managers = value; }
        public ConcurrentDictionary<string, Owner> Owners { get => owners; set => owners = value; }
        public Founder Founder { get => founder; set => founder = value; }
        public object Prem_lock { get => prem_lock; set => prem_lock = value; }

        public Store(string name, BankAccount bank, Address address)
        {
            this.name = name;
            this._products = new ConcurrentDictionary<string, Product>();
            this._transactionsHistory = new HashSet<TransactionStatus>();
            this._lock = new object();
            this.prem_lock = new object();
            this.Discounts = new HashSet<Discount>();
            this._id = Guid.NewGuid();
            this._policy = new Policy();
            this._bank = bank;
            this._address = address;
            this.managers = new ConcurrentDictionary<string, Manager>();
            this.owners = new ConcurrentDictionary<string, Owner>();
            this.History = new HashSet<IHistory>();
        }

        public Guid GetId()
        {
            return _id;
        }
        public bool isStaff(string username)
        {
            return founder.Username.Equals(username) || managers.ContainsKey(username) || owners.ContainsKey(username);
        }
        public PurchaseStatus Purchase(IShoppingBasket shoppingBasket, string username, string clientPhone, Address clientAddress, PaymentMethod method, double paymentSum)
        {
            bool enoughtQuantity;
            TransactionStatus transactionStatus;
            Dictionary<Product, int> product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            lock (_lock)
            {
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, _id);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = _transaction.ActivateTransaction(username, clientPhone, weight, _address, clientAddress, method, _id, _bank, paymentSum, shoppingBasket);
            history.Add(new TransactionHistory(transactionStatus));
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            return new PurchaseStatus(true, transactionStatus, _id);
        }

        public void CancelTransaction(Dictionary<Product, int> product_quantity)
        {
            UpdateQuantities(product_quantity, false);
        }

        
        public double CalcPaySum(IShoppingBasket shoppingBasket)
        {
            double discount = ApplyDiscounts(shoppingBasket);
            double cost = shoppingBasket.CalcCost();
            return cost - discount;
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
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
        public String AddProduct(Product product, string userID)
        {
            if (!founder.Username.Equals(userID) || owners.ContainsKey(userID))
            {
                Manager m;
                if(!managers.TryGetValue(userID, out m))
                    return "Invalid user";
                else if(m.GetPermission(Permission.AddProduct))
                    return "No permission";
            }
            if (!validProduct(product))
                return "Invalid product";
            if (_products.ContainsKey(product.Name))
                return "Product exists";
            _products.TryAdd(product.Name, product);
            return "Product added";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(String productName, string userID)
        {
            if (!founder.Username.Equals(userID) || owners.ContainsKey(userID))
            {
                Manager m;
                if (!managers.TryGetValue(userID, out m))
                    return "Invalid user";
                else if (m.GetPermission(Permission.AddProduct))
                    return "No permission";
            }
            Product useless;
            _products.TryRemove(productName, out useless);
            return "Product removed";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(String productName, Product editedProduct, string userID)
        {
            if (!founder.Username.Equals(userID) || owners.ContainsKey(userID))
            {
                Manager m;
                if (!managers.TryGetValue(userID, out m))
                    return "Invalid user";
                else if (m.GetPermission(Permission.AddProduct))
                    return "No permission";
            }
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

        public String AssignMember(string assigneeID, User assigner, string type)
        {
            Owner assignerOwner;
            Appointer a;
            MemberState assignee;
            string ret;
            if (managers.ContainsKey(assigner.Username))
                return "Invalid assigner";
            else if (founder.Username.Equals(assigner.Username))
                a = founder;
            else if (owners.TryGetValue(assigner.Username, out assignerOwner))
                a = assignerOwner;
            else
                return "Invalid assigner";
            if (!MarketUsers.Instance.MemberStates.TryGetValue(assigneeID, out assignee))
                return "the assignee isn't a member";
            lock (assignee.Prem_lock)
            {
                try
                {
                    if (type.Equals("owner"))
                    {
                        a.AddAppointmentOwner(assignee, this);
                    }
                    else
                    {
                        a.AddAppointmentManager(assignee, this);
                    }
                    ret = "Success";
                }
                catch { ret="this member is already assigned as a store owner or manager"; }
            }
            

            return ret;
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> permissionsToRemove, List<Permission> permissionsToAdd)
        {
            Manager manager;
            if (!managers.TryGetValue(managerID, out manager))
                return "Manager doesn't exist";
            Owner assignerOwner;
            Appointer a;
            MemberState assignee;
            string ret;
            if (managers.ContainsKey(assignerID))
                return "Invalid assigner";
            else if (founder.Username.Equals(assignerID))
                a = founder;
            else if (owners.TryGetValue(assignerID, out assignerOwner))
                a = assignerOwner;
            else
                return "Invalid assigner";
            foreach(Permission p in permissionsToAdd)
            {
                try
                {
                    a.AddPermission(managerID, p);
                }
                catch
                {
                    return "Invalid assigner";
                }
                
            }
            foreach (Permission p in permissionsToRemove)
            {
                try
                {
                    a.RemovePermission(managerID, p);
                }
                catch
                {
                    return "Invalid assigner";
                }

            }

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
        public ICollection<IHistory> GetStoreHistory(string username)
        {
            bool isPermitted = CheckPermission(username, Permission.GetShopHistory);
            if (isPermitted)
            {
                return History;
            }
            throw new UnauthorizedAccessException();
        }

        private bool CheckPermission(string username, Permission permission)
        {
            Manager manager;
            managers.TryGetValue(username, out manager);
            Owner owner;
            owners.TryGetValue(username, out owner);
            return ((manager != null && manager.GetPermission(permission)) || (owner != null) || founder.Username.Equals(username));
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
