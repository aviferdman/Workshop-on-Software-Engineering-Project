using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class Store 
    {
        public HashSet<Product> _products { get; set; }
        public Founder founder { get; set; }
        public HashSet<Manager> managers { get; set; }
        public HashSet<Owner> owners { get; set; }
        public CreditCard _bank { get; set; }
        public Guid sid { get; set; }
        public string name { get; set; }
        public ICollection<Discount> _discounts { get; set; }
        public Policy _policy { get; set; }
        public Address _address { get; set; }
        public ICollection<Bid> bids { get; set; }

        public HashSet<Product> Products { get => _products; set => _products = value; }
        internal Policy Policy { get => _policy; set => _policy = value; }
        internal Address Address { get => _address; set => _address = value; }
        public Guid Id { get => sid; set => sid = value; }
        public string Name { get => name; set => name = value; }
        internal CreditCard Bank { get => _bank; set => _bank = value; }
        public ICollection<Discount> Discounts { get => _discounts; set => _discounts = value; }
        public HashSet<Manager> Managers { get => managers; set => managers = value; }
        public HashSet<Owner> Owners { get => owners; set => owners = value; }
        public Founder Founder { get => founder; set => founder = value; }
        public string Name1 { get => name; set => name = value; }

        public Store()
        {

        }

        public Store(string name, CreditCard bank, Address address)
        {
            this.name = name;
            this._products = new HashSet<Product>();
            this.Discounts = new HashSet<Discount>();
            this.sid = Guid.NewGuid();
            this._policy = new Policy();
            this._bank = bank;
            this._address = address;
            this.managers = new HashSet<Manager>();
            this.owners = new HashSet<Owner>();
            this.bids = new HashSet<Bid>();
        }

        public virtual Guid GetId()
        {
            return sid;
        }
        public bool hasPremssion(string username, Permission p)
        {
            if ((!founder.Username.Equals(username)) && !(owners.Where(o => o.username.Equals(username)).Any()))
            {

                if (!managers.Where(m => m.username.Equals(username)).Any())
                    return false;
                else {
                    Manager m = managers.Where(m => m.username.Equals(username)).First();
                    if (!m.GetPermission(p))
                        return false;
                }
            }
            return true;
        }
        public bool isStaff(string username)
        {
            return ((!founder.Username.Equals(username)) && !(owners.Where(o => o.username.Equals(username)).Any()) && !(managers.Where(m => m.username.Equals(username)).Any())) ;
        }

        public HashSet<Owner> GetOwners()
        {
            return owners;
        }

        //TODO
        public virtual async Task<PurchaseStatus> Purchase(ShoppingBasket shoppingBasket, string clientPhone, Address clientAddress, PaymentMethod method)
        {
            bool enoughtQuantity;
            TransactionStatus transactionStatus;
            HashSet<ProductInCart> product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            string username = shoppingBasket.GetShoppingCart().GetUser().Username;
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.product.Weight * next.quantity);
            double paySum = CalcPrice(username, shoppingBasket);
            lock (this)
            {
                if (!CheckPolicy(shoppingBasket)) return new PurchaseStatus(false, null, sid);
                enoughtQuantity = EnoughQuantity(product_quantity);
                //pre-conditions not legal
                if (!enoughtQuantity) return new PurchaseStatus(false, null, sid);

                UpdateQuantities(product_quantity);
            }
            transactionStatus = await Transaction.Instance.ActivateTransaction(username, clientPhone, weight, _address, clientAddress, method, sid, _bank, paySum, shoppingBasket);
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
            return new PurchaseStatus(true, transactionStatus, sid);
        }

        //TODO
        public virtual double CalcPrice(string username, ShoppingBasket shoppingBasket)
        {
            double paySum = CalcPaySum(shoppingBasket);
            return paySum - CalcBidNewSum(username, shoppingBasket);
        }

        //TODO
        private double CalcBidNewSum(string username, ShoppingBasket shoppingBasket)
        {
            double sum = 0;
            foreach (var p_q in shoppingBasket.GetDictionaryProductQuantity())
            {
                sum += GetProductBid(username, p_q.product) * p_q.quantity;
            }
            return sum;
        }

        //TODO
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
            HistoryManager.Instance.AddHistory(transactionStatus);
        }

        private void NotifyOwners()
        {
            //notify the founder for a new purchase
            PublisherManagement.Instance.EventNotification(founder.Username, EventType.PurchaseEvent, ConfigurationManager.AppSettings["PurchaseMessage"]);

            //notify the owners
            foreach (var owner in owners)
            {
                PublisherManagement.Instance.EventNotification(owner.Username, EventType.PurchaseEvent, ConfigurationManager.AppSettings["PurchaseMessage"]);
            }
        }

        //TODO
        public void CancelTransaction(HashSet<ProductInCart> product_quantity)
        {
            UpdateQuantities(product_quantity, false);
        }

        //TODO
        public double CalcPaySum(ShoppingBasket shoppingBasket)
        {
            double discount = ApplyDiscounts(shoppingBasket);
            double cost = shoppingBasket.CalcCost();
            return cost - discount;
        }
        //TODO
        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public virtual double ApplyDiscounts(ShoppingBasket shoppingBasket)
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
        //TODO
        //use case 12 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/75
        public virtual bool CheckPolicy(ShoppingBasket shoppingBasket)
        {
            return Policy.Check(shoppingBasket);
        }
        //TODO
        public void SetPolicy(string userID, Policy policy)
        {
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return;
            this.Policy = policy;
        }

        public Policy GetPolicy()
        {
            return Policy;
        }

        //TODO
        public Guid AddRule(string userID, IRule rule)
        {
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return new Guid();
            _policy.AddRule(rule);
            return rule.GetId();
        }

        //TODO
        public void RemoveRule(string userID)
        {
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return;
            _policy.RemoveRule();
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String AddProduct(Product product, string userID)
        {
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.AddProduct))
                return "No permission";
            if (!validProduct(product))
                return "Invalid product";
            lock (_products)
            {
                if (_products.Where(p => p.Id.Equals(product.Id)).Any())
                    return "Product exists";
                _products.Add(product);
                MarketStores.Instance.addToCategory(product, product.Category);
            }
            ProxyMarketContext.Instance.saveChanges();
            return "Product added";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(Guid productID, string userID)
        {
            Product toRem;
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.AddProduct))
                return "No permission";
            lock (_products)
            {
                if (!_products.Where(p => p.Id.Equals(productID)).Any())
                    return "Product doesn't exist";
                toRem = _products.Where(p => p.Id.Equals(productID)).First();
                MarketStores.Instance.removeFromCategory(toRem, toRem.Category);
                _products.Remove(toRem);
                MarketDAL.Instance.RemoveProduct(toRem);
            }
            return "Product removed";
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(Guid productID, Product editedProduct, string userID)
        {
            Product prev;
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.EditProduct))
                return "No permission";
            lock (_products)
            {
                if (!_products.Where(p => p.Id.Equals(productID)).Any())
                    return "Product not in the store";
                if (!validProduct(editedProduct))
                    return "Invalid edit";
                prev= _products.Where(p => p.Id.Equals(productID)).First();
                //TODO add edit categ
                MarketStores.Instance.removeFromCategory(prev, prev.Category);
                prev.category = editedProduct.category;
                prev.Name = editedProduct.Name;
                prev.Price = editedProduct.Price;
                prev.Quantity = editedProduct.Quantity;
                prev.Rating = editedProduct.rating;
                prev.Weight = editedProduct.Weight;
            }
            ProxyMarketContext.Instance.saveChanges();
            return "Product edited";
        }

        public async Task<String> AssignMember(string assigneeID, User assigner, string type)
        {
            Owner assignerOwner;
            Appointer a;
            MemberState assignee;
            string ret;
            if (assigner == null)
                return "invalid assiner";
            if (managers.Where(m=> m.username.Equals(assigner.Username)).Any())
                return "Invalid assigner";
            else if (founder.Username.Equals(assigner.Username))
                a = founder;
            else if (owners.Where(p => p.username.Equals(assigner.Username)).Any())
                a = owners.Where(p => p.username.Equals(assigner.Username)).First();
            else
                return "Invalid assigner";
            assignee = await MarketDAL.Instance.getMemberState(assigneeID);
            if (assignee==null)
                return "the assignee isn't a member";
            lock (this)
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
            
            return ret;
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(string managerID, string assignerID, List<Permission> newPermissions)
        {
            Manager manager;
            if (!managers.Where(m=>m.username.Equals(managerID)).Any())
                return "Manager doesn't exist";
            manager = managers.Where(m => m.username.Equals(managerID)).First();
            Appointer a;
            if (!managers.Where(m => m.username.Equals(assignerID)).Any())
                return "Invalid assigner";
            else if (founder.Username.Equals(assignerID))
                a = founder;
            else if (owners.Where(p => p.username.Equals(assignerID)).Any())
                a = owners.Where(p => p.username.Equals(assignerID)).First();
            else
                return "Invalid assigner";
            try
            {
                a.DefinePermissions(managerID, manager,newPermissions);
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
            String ret;
            Manager manager;
            if (!managers.Where(m => m.username.Equals(managerName)).Any())
                return "Manager doesn't exist";
            manager = managers.Where(m => m.username.Equals(managerName)).First();
            if (founder.Username.Equals(assigner))
                a = founder;
            else if (owners.Where(p => p.username.Equals(assigner)).Any())
                a = owners.Where(p => p.username.Equals(assigner)).First();
            else
                return "Invalid assigner";
            if (!a.canRemoveAppointment(managerName))
                return "Invalid Assigner";
            lock (this)
            {
                manager.removePermission(this);
                MarketDAL.Instance.removeManager(manager);
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
            Owner ownerToRemove;
            if (!owners.Where(o=>o.username.Equals(ownerName)).Any())
            {
                return "Owner doesn't exist";
            }
            ownerToRemove = owners.Where(o => o.username.Equals(ownerName)).Single();
            if (founder.Username.Equals(assigner))
                a = founder;
            else if (owners.Where(p => p.username.Equals(assigner)).Any())
                a = owners.Where(p => p.username.Equals(assigner)).First();
            else
                return "Invalid assigner";
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
            lock (this)
            {
                ownerToRemove.removePermission(this);
                owners.Remove(ownerToRemove);
                MarketDAL.Instance.removeOwner(ownerToRemove);
                ret = "success";
                PublisherManagement.Instance.EventNotification(ownerName, EventType.RemoveAppointment, assigner + " has revoked your appointment as an owner for store " + name);
            }

            return ret;
        }

        //functional requirement 4.9 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/60
        public List<WorkerDetails> GetInfo(String username)
        {
            List<WorkerDetails> ret = new List<WorkerDetails>();
            if ((!founder.Username.Equals(username)) && !(owners.Where(o => o.username.Equals(username)).Any()) && !(managers.Where(m => m.username.Equals(username)).Any()))
                return ret;
            if (!hasPremssion(username, Permission.GetPersonnelInfo))
                return ret;
            ret.Add(new WorkerDetails(founder));
            foreach (Owner owner in owners)
            {
                ret.Add(new WorkerDetails(owner));
            }
            foreach (Manager manager in managers)
            {
                ret.Add(new WorkerDetails(manager));
            }
            return ret;
        }

        public WorkerDetails GetInfoSpecific(String workerName, String username)
        {
            WorkerDetails ret = null;
            if ((!founder.Username.Equals(username)) && !(owners.Where(o => o.username.Equals(username)).Any()) && !(managers.Where(m => m.username.Equals(username)).Any()))
                return ret;
            if (!hasPremssion(username, Permission.GetPersonnelInfo))
                return ret;
            if (founder.Username.Equals(workerName))
                return new WorkerDetails(founder);
            else if (owners.Where(o => o.username.Equals(workerName)).Any())
                return new WorkerDetails(owners.Where(o => o.username.Equals(workerName)).Single());
            else if (managers.Where(o => o.username.Equals(workerName)).Any())
            {
                return new WorkerDetails(managers.Where(o => o.username.Equals(workerName)).Single());
            }
            return null;
        }

        //for tests
        public void UpdateProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p!=null){    //remove old product if exists
                _products.Remove(_products.Where(p=> p.Id.Equals(product.Id)).Single());
            }

            _products.Add(product); //add the new product
        }

        //for tests
        public void RemoveProduct(Product product)
        {
            Product p = GetProductById(product.Id);
            if (p != null)
            {    //remove old product if exists
                _products.Remove(p);
            }
        }

        public Guid AddDiscount(string userID, Discount discount)
        {
            //Have no permission to do the action
            if((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            Discounts.Add(discount);
            return discount.Id;
        }

        //TODO
        public Guid RemoveDiscount(string userID, Guid discountId)
        {
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            Discount d = GetDiscountById(discountId);
            if (d != null)
            {    //remove old discount if exists
                Discounts.Remove(d);
            }
            return discountId;
        }

        //TODO
        private void UpdateQuantities(HashSet<ProductInCart> product_quantity, bool subOrAdd = true)
        {
            foreach (ProductInCart p_q in product_quantity)
            {
                Product product = GetProductById(p_q.product.Id);
                if (subOrAdd)
                {   
                    product.Quantity = product.Quantity - p_q.quantity;
                }
                else
                {
                    product.Quantity = product.Quantity + p_q.quantity;
                }
            }
        }
        //TODO
        private bool EnoughQuantity(HashSet<ProductInCart> product_quantity)
        {
            bool enoughtQuantity = true;
            foreach (ProductInCart p_q in product_quantity)
            {
                Guid productId = p_q.product.Id;
                Product p = GetProductById(productId);
                enoughtQuantity = enoughtQuantity && p != null && p.Quantity >= p_q.quantity;
            }
            return enoughtQuantity;
        }

        private Product GetProductById(Guid productId)
        {
            IEnumerable<Product> products = Products.Where(product => product.Id.Equals(productId));
            return products.FirstOrDefault();
        }
        //TODO
        public Discount GetDiscountById(Guid discountId)
        {
            IEnumerable<Discount> discounts = Discounts.Where(discount => discount.Id.Equals(discountId));
            return discounts.FirstOrDefault();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public async Task<ICollection<IHistory>> GetStoreHistory(string username)
        {
            bool isPermitted = CheckPermission(username, Permission.GetShopHistory);
            if (isPermitted)
            {
                return await HistoryManager.Instance.GetStoreHistory(Id);
            }
            throw new UnauthorizedAccessException();
        }

        private bool CheckPermission(string username, Permission permission)
        {
            return ((managers.Where(m => m.username.Equals(username) && m.GetPermission(permission)).Any() || owners.Where(o=> o.username.Equals(username)).Any()|| founder.Username.Equals(username)));
        }

        public int CompareTo(object obj)
        {
            return obj.GetHashCode() - GetHashCode();
        }

        //TODO
        public IRule GetRuleById(Guid ruleId)
        {
            if (this.Policy.Rule.GetId().Equals(ruleId))
            {
                return this.Policy.Rule;
            }
            return null;
        }
        //TODO
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
        //TODO
        public Result<bool> AcceptBid(string ownerUsername, string username, Guid productId, double newBidPrice)
        {
            if (!CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            this.bids.Add(new Bid(username, productId, newBidPrice));
            return new Result<bool>(true, false, "");
        }

        public bool Contains(String comparator, String list)
        {
            switch (list)
            {
                case "Owners":
                    return Owners.Where(o => o.Username.Equals(comparator)).Any();
                case "Managers":
                    return Managers.Where(m => m.Username.Equals(comparator)).Any();
                case "Products":
                    return Products.Where(p => p.Id.Equals(comparator)).Any();
                default: return false;
            }
        }

        public Manager GetManager(String name)
        {
            foreach (Manager manager in Managers)
            {
                if (manager.Username.Equals(name))
                    return manager;
            }
            return null;
        }

        public Owner GetOwner(String name)
        {
            foreach (Owner owner in Owners)
            {
                if (owner.Username.Equals(name))
                    return owner;
            }
            return null;
        }

        public Product GetProduct(Guid id)
        {
            foreach (Product product in Products)
            {
                if (product.Id.Equals(id))
                    return product;
            }
            return null;
        }
    }
}
