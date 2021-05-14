using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class Store : IStore
    {
        private ConcurrentDictionary<Guid, Product> _products;
        private ICollection<TransactionStatus> _transactionsHistory;
        private Founder founder;
        private ConcurrentDictionary<string, IManager> managers;
        private ConcurrentDictionary<string, Owner> owners;
        private BankAccount _bank;
        private Guid _id;
        private string name;
        private ICollection<Discount> _discounts;
        private Policy _policy;
        private Address _address;
        private ICollection<IHistory> history;
        private ICollection<Bid> bids;
        private object _lock;
        private object prem_lock;

        public ConcurrentDictionary<Guid, Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        internal ICollection<TransactionStatus> TransactionsHistory { get => _transactionsHistory; set => _transactionsHistory = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => name; set => name = value; }
        internal BankAccount Bank { get => _bank; set => _bank = value; }
        public ICollection<Discount> Discounts { get => _discounts; set => _discounts = value; }
        public ICollection<IHistory> History { get => history; set => history = value; }
        public ConcurrentDictionary<string, IManager> Managers { get => managers; set => managers = value; }
        public ConcurrentDictionary<string, Owner> Owners { get => owners; set => owners = value; }
        public Founder Founder { get => founder; set => founder = value; }
        public object Prem_lock { get => prem_lock; set => prem_lock = value; }
        public string Name1 { get => name; set => name = value; }

        public Store(string name, BankAccount bank, Address address)
        {
            this.name = name;
            this._products = new ConcurrentDictionary<Guid, Product>();
            this._transactionsHistory = new HashSet<TransactionStatus>();
            this._lock = new object();
            this.prem_lock = new object();
            this.Discounts = new HashSet<Discount>();
            this._id = Guid.NewGuid();
            this._policy = new Policy();
            this._bank = bank;
            this._address = address;
            this.managers = new ConcurrentDictionary<string, IManager>();
            this.owners = new ConcurrentDictionary<string, Owner>();
            this.History = new HashSet<IHistory>();
            this.bids = new HashSet<Bid>();
        }

        public Guid GetId()
        {
            return _id;
        }
        public bool hasPremssion(string username, Permission p)
        {
            if (!founder.Username.Equals(username) && !owners.ContainsKey(username))
            {
                IManager m;
                if (!managers.TryGetValue(username, out m))
                    return false;
                else if (!m.GetPermission(p))
                    return false;
            }
            return true;
        }
        public bool isStaff(string username)
        {
            return (founder!=null&&founder.Username.Equals(username)) || managers.ContainsKey(username) || owners.ContainsKey(username);
        }

        public ConcurrentDictionary<string, Owner> GetOwners()
        {
            return owners;
        }
        public async Task<PurchaseStatus> Purchase(IShoppingBasket shoppingBasket, string clientPhone, Address clientAddress, PaymentMethod method)
        {
            bool enoughtQuantity;
            TransactionStatus transactionStatus;
            Dictionary<Product, int> product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            string username = shoppingBasket.GetShoppingCart().GetUser().Username;
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            double paySum = CalcPrice(username, shoppingBasket);
            lock (_lock)
            {
                if (!CheckPolicy(shoppingBasket)) return new PurchaseStatus(false, null, _id);
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, _id);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = await Transaction.Instance.ActivateTransaction(username, clientPhone, weight, _address, clientAddress, method, _id, _bank, paySum, shoppingBasket);
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            //add to history
            else
            {
                AddToHistory(transactionStatus);
                NotifyOwners();
            }
            return new PurchaseStatus(true, transactionStatus, _id);
        }

        public double CalcPrice(string username, IShoppingBasket shoppingBasket)
        {
            double paySum = CalcPaySum(shoppingBasket);
            return paySum - CalcBidNewSum(username, shoppingBasket);
        }

        private double CalcBidNewSum(string username, IShoppingBasket shoppingBasket)
        {
            double sum = 0;
            foreach (var p_q in shoppingBasket.GetDictionaryProductQuantity())
            {
                sum += GetProductBid(username, p_q.Key) * p_q.Value;
            }
            return sum;
        }

        private double GetProductBid(string username, Product p)
        {
            double sum = 0;
            foreach (var bid in bids)
            {
                if (bid.Username.Equals(username) && bid.ProductId.Equals(p.Id))
                {
                    sum += bid.Price < p.Price ? p.Price - bid.Price : 0;
                }
            }
            return sum;
        }

        private void AddToHistory(TransactionStatus transactionStatus)
        {
            var h = new StoreHistory(transactionStatus);
            history.Add(h);
            HistoryManager.Instance.AddUserHistory(h);
        }

        private void NotifyOwners()
        {
            //notify the founder for a new purchase
            PublisherManagement.Instance.EventNotification(founder.Username, EventType.PurchaseEvent, ConfigurationManager.AppSettings["PurchaseMessage"]);

            //notify the owners
            foreach (var owner in owners.Values)
            {
                PublisherManagement.Instance.EventNotification(owner.Username, EventType.PurchaseEvent, ConfigurationManager.AppSettings["PurchaseMessage"]);
            }
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
            var availableDiscounts = Discounts.Select(d=>d.ApplyDiscounts(shoppingBasket));
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
            return Policy.Check(shoppingBasket);
        }

        public void SetPolicy(string userID, Policy policy)
        {
            if (((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID))) || !hasPremssion(userID, Permission.EditPolicy))
                return;
            this.Policy = policy;
        }

        public Policy GetPolicy()
        {
            return Policy;
        }

        public Guid AddRule(string userID, IRule rule)
        {
            //Have no permission to do the action
            if (((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID))) || !hasPremssion(userID, Permission.EditPolicy))
                return new Guid();
            _policy.AddRule(rule);
            return rule.GetId();
        }

        public void RemoveRule(string userID)
        {
            //Have no permission to do the action
            if (((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID))) || !hasPremssion(userID, Permission.EditPolicy))
                return;
            _policy.RemoveRule();
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String AddProduct(Product product, string userID)
        {
            if ((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID)))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.AddProduct))
                return "No permission";
            if (!validProduct(product))
                return "Invalid product";
            if (!_products.TryAdd(product.Id, product))
                return "Product exists";
            MarketStores.Instance.addToCategory(product, product.Category);
            return "Product added";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(Guid productID, string userID)
        {
            Product toRem;
            if ((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID)))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.AddProduct))
                return "No permission";
            if (!_products.TryRemove(productID, out toRem))
                return "Product doesn't exist";
            MarketStores.Instance.removeFromCategory(toRem, toRem.Category);
            return "Product removed";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(Guid productID, Product editedProduct, string userID)
        {
            Product prev;
            if ((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID)))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.EditProduct))
                return "No permission";
            if (!_products.TryRemove(productID, out prev))
                return "Product not in the store";
            if (!validProduct(editedProduct))
                return "Invalid edit";
            MarketStores.Instance.removeFromCategory(prev, prev.Category);
            editedProduct.Id = productID;
            _products.TryAdd(productID, editedProduct);
            MarketStores.Instance.addToCategory(editedProduct, editedProduct.Category);
            return "Product edited";
        }

        public String AssignMember(string assigneeID, User assigner, string type)
        {
            Owner assignerOwner;
            Appointer a;
            MemberState assignee;
            string ret;
            if (assigner == null)
                return "invalid assiner";
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
                lock (prem_lock)
                {
                    try
                    {
                        if (type.Equals("owner"))
                        {
                            a.AddAppointmentOwner(assignee, this);
                            PublisherManagement.Instance.EventNotification(assigneeID, EventType.AddAppointmentEvent, assigner.Username + " has appointed you as an owner for store " + name);
                        }
                        else
                        {
                            a.AddAppointmentManager(assignee, this);
                            PublisherManagement.Instance.EventNotification(assigneeID, EventType.AddAppointmentEvent, assigner.Username + " has appointed you as a manager for store "+name);
                        }

                        ret = "Success";
                        
                    }
                    catch { ret = "this member is already assigned as a store owner or manager"; }
                }
            }
            return ret;
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> newPermissions)
        {
            IManager manager;
            if (!managers.TryGetValue(managerID, out manager))
                return "Manager doesn't exist";
            Owner assignerOwner;
            Appointer a;
            if (managers.ContainsKey(assignerID))
                return "Invalid assigner";
            else if (founder.Username.Equals(assignerID))
                a = founder;
            else if (owners.TryGetValue(assignerID, out assignerOwner))
                a = assignerOwner;
            else
                return "Invalid assigner";
            try
            {
                a.DefinePermissions(managerID, newPermissions);
            }
            catch {return "Invalid assigner";}

            return "Success";
        }

        private bool validProduct(Product product)
        {
            return !(product.Name == null || product.Name.Trim().Equals("") || product.Quantity <= 0 || product.Weight < 0 
                || product.Price <= 0 || product.Category == null || product.Category.Trim().Equals(""));

        }

        //functional requirement 4.7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/57
        public String RemoveManager(String managerName, String assigner)
        {
            Appointer a;
            Owner owner;
            String ret;
            if (!managers.TryGetValue(managerName, out IManager manager))
            {
                return "Manager doesn't exist";
            }
            if (founder.Username.Equals(assigner))
                a = founder;
            else if (owners.TryGetValue(assigner, out owner))
                a = owner;
            else return "Invalid Assigner";
            if(!a.canRemoveAppointment(managerName))
                return "Invalid Assigner";
            lock (prem_lock)
            {
                a.removeAppointment(managerName);
                manager.removePermission(this);
                managers.TryRemove(managerName, out _);
                ret = "success";
                PublisherManagement.Instance.EventNotification(managerName, EventType.RemoveAppointment, assigner + " has revoked your appointment as a manager for store " + name);

            }

            return ret;
        }

        //functional requirement 4.4 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/136
        public String RemoveOwner(String ownerName, String assigner)
        {
            Appointer a;
            String ret;
            if (!owners.TryGetValue(ownerName, out Owner ownerToRemove))
            {
                return "Owner doesn't exist";
            }
            if (founder.Username.Equals(assigner))
                a = founder;
            else if (owners.TryGetValue(assigner, out Owner owner))
                a = owner;
            else return "Invalid Assigner";
            if (!a.canRemoveAppointment(ownerName))
                return "Invalid Assigner";
            
            if (ownerToRemove.hasAppointees())
            {
                foreach (String appointee in ownerToRemove.getAppointees())
                {
                    ret = RemoveOwner(appointee, ownerName);
                    if (!ret.Equals("success"))
                        return ret;
                }
            }
            ownerToRemove.removeManagers();
            lock (prem_lock)
            {
                ownerToRemove.removePermission(this);
                owners.TryRemove(ownerName, out _);
                ret = "success";
                PublisherManagement.Instance.EventNotification(ownerName, EventType.RemoveAppointment, assigner + " has revoked your appointment as an owner for store " + name);
            }

            return ret;
        }

        //functional requirement 4.9 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/60
        public String getInfo(String username)
        {
            String ret;
            if ((!founder.Username.Equals(username)) && !(owners.ContainsKey(username)) && !(managers.ContainsKey(username)))
                return "Invalid user";
            if (!hasPremssion(username, Permission.GetPersonnelInfo))
                return "No permission";
            ret = "Founder - " + founder.Username + "\n";
            foreach (Owner owner in owners.Values)
            {
                ret = ret + "Owner - " + owner.Username + "\n";
            }
            foreach (Manager manager in managers.Values)
            {
                WorkerDetails details = new WorkerDetails(manager);
                ret = ret + details.toString() + "\n";
            }
            return ret;
        }

        public String getInfoSpecific(String workerName, String username)
        {
            String ret;
            if ((!founder.Username.Equals(username)) && !(owners.ContainsKey(username)) && !(managers.ContainsKey(username)))
                return "Invalid user";
            if (!hasPremssion(username, Permission.GetPersonnelInfo))
                return "No permission";
            if (founder.Username.Equals(workerName))
                return "Founder - " + founder.Username;
            else if (owners.TryGetValue(workerName, out Owner owner))
                return "Owner - " + workerName;
            else if (managers.TryGetValue(workerName, out IManager manager))
            {
                WorkerDetails details = new WorkerDetails((Manager)manager);
                return details.toString();
            }
            return "Worker not found";
        }

        public void UpdateProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p!=null){    //remove old product if exists
                Product useless;
                _products.TryRemove(product.Id, out useless);
            }

            _products.TryAdd(product.Id, product); //add the new product
        }

        public void RemoveProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p != null)
            {    //remove old product if exists
                Product useless;
                _products.TryRemove(product.Id, out useless);
            }
        }

        public Guid AddDiscount(string userID, Discount discount)
        {
            //Have no permission to do the action
            if (((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID))) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            Discounts.Add(discount);
            return discount.Id;
        }

        public Guid RemoveDiscount(string userID, Guid discountId)
        {
            //Have no permission to do the action
            if (((!founder.Username.Equals(userID)) && !(owners.ContainsKey(userID)) && !(managers.ContainsKey(userID))) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            Discount d = GetDiscountById(discountId);
            if (d != null)
            {    //remove old discount if exists
                Discounts.Remove(d);
            }
            return discountId;
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

        public Discount GetDiscountById(Guid discountId)
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
            IManager manager;
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
                if((p.Name!=null&&p.Name.Contains(keyword))||(p.Category!=null&& category!=null&& p.Category.Contains(keyword)))
                {
                    if (category != null&& !category.Equals(p.Category))
                        continue;
                    if (price_range_low != -1 && price_range_low > p.Price)
                        continue;
                    if (price_range_high != -1 && price_range_high < p.Price)
                        continue;
                    if (rating != -1 && rating != p.Rating)
                        continue;
                    products.Add(p);
                }
            }
            return products;
        }

        public IRule GetRuleById(Guid ruleId)
        {
            if (this.Policy.Rule.GetId().Equals(ruleId))
            {
                return this.Policy.Rule;
            }
            return null;
        }

        public IRule GetDiscountRuleById(Guid ruleId)
        {
            return Discounts.Where(d => d.GetRule().GetId().Equals(ruleId)).FirstOrDefault().GetRule();
        }

        public Founder GetFounder()
        {
            return founder;
        }

        public void SetFounder(Founder f)
        {
            this.founder = f;
        }

        public Result<bool> AcceptBid(string ownerUsername, string username, Guid productId, double newBidPrice)
        {
            if (!CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            this.bids.Add(new Bid(username, productId, newBidPrice));
            return new Result<bool>(true, false, "");
        }
    }
}
