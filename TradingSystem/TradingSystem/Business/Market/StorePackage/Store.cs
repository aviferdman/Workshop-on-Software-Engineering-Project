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
        [NotMapped]
        public CreditCard _bank { get; set; }
        public Guid sid { get; set; }
        public string name { get; set; }
        public Address _address { get; set; }
        [NotMapped]
        public HashSet<Product> Products { get => _products; set => _products = value; }
        [NotMapped]
        internal Address Address { get => _address; set => _address = value; }
        [NotMapped]
        public Guid Id { get => sid; set => sid = value; }
        [NotMapped]
        public string Name { get => name; set => name = value; }
        [NotMapped]
        public CreditCard Bank { get => _bank; set => _bank = value; }
        [NotMapped]
        public HashSet<Manager> Managers { get => managers; set => managers = value; }
        [NotMapped]
        public HashSet<Owner> Owners { get => owners; set => owners = value; }
        [NotMapped]
        public Founder Founder { get => founder; set => founder = value; }
        public PurchasePolicy purchasePolicy { get; set; }

        public Store()
        {

        }

        public Store(string name, CreditCard bank, Address address)
        {
            this.name = name;
            this._products = new HashSet<Product>();
            this.sid = Guid.NewGuid();
            this._bank = bank;
            this._address = address;
            this.managers = new HashSet<Manager>();
            this.owners = new HashSet<Owner>();
            this.purchasePolicy = new PurchasePolicy();
        }

        internal async Task CustomerDenyBid(Guid bidId)
        {
            founder.CustomerDenyBid(bidId);
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

        public  Result<WorkerDetails> GetPerms(string username)
        {
            if (founder.Username.Equals(username))
                return new Result<WorkerDetails>(new WorkerDetails(founder) ,false, null);
            else if (owners.Where(o => o.username.Equals(username)).Any())
                return new Result<WorkerDetails>(new WorkerDetails(owners.Where(o => o.username.Equals(username)).Single()), false,null);
            else if (managers.Where(o => o.username.Equals(username)).Any())
            {
                return new Result<WorkerDetails>(new WorkerDetails(managers.Where(o => o.username.Equals(username)).Single()), false,null);
            }
            return new Result<WorkerDetails>(null,true, "user doesnt exist");
        }

        public bool isStaff(string username)
        {
            return ((founder.Username.Equals(username)) ||(owners.Where(o => o.username.Equals(username)).Any()) || (managers.Where(m => m.username.Equals(username)).Any())) ;
        }

        public HashSet<Owner> GetOwners()
        {
            return owners;
        }

        // TODO
        public async Task AddBid(Bid bid)
        {
            await founder.AddBid(bid);
            foreach (var owner in Owners)
            {
                await owner.AddBid(bid);
            }
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
            transactionStatus = await Transaction.Instance.ActivateTransaction(username, clientPhone, weight, _address, clientAddress, method, sid, name, _bank, paySum, shoppingBasket);
            //transaction failed
            if (!transactionStatus.Status)
            {
                CancelTransaction(product_quantity);
            }
            //add to history
            else
            {
                AddToHistory(transactionStatus);
                NotifyOwners(EventType.PurchaseEvent, username + " purchased items from store " + name);
            }
            return new PurchaseStatus(true, transactionStatus, sid);
        }

        public async Task<Result<bool>> ChangeBidPolicy(bool isAvailable)
        {
            if (isAvailable)
            {
                await this.purchasePolicy.AddPurchaseKind(PurchaseKind.Bid);
                return new Result<bool>(true, false, "");
            }
            else
            {
                await this.purchasePolicy.RemovePurchaseKind(PurchaseKind.Bid);
                return new Result<bool>(true, false, "");
            }
        }

        public bool IsPurchaseKindAvailable(PurchaseKind purchaseKind)
        {
            return this.purchasePolicy.IsAvalable(purchaseKind);
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
            var bids = founder == null ? new HashSet<Bid>() : founder.GetUserAcceptedBids(username);
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

        public void NotifyOwners(EventType eventType, string message)
        {
            //notify the founder for a new purchase
            PublisherManagement.Instance.EventNotification(founder.Username, eventType, message);

            //notify the owners
            foreach (var owner in owners)
            {
                PublisherManagement.Instance.EventNotification(owner.Username, eventType, message);
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
            var discounts = StorePredicatesManager.Instance.GetDiscounts(Id);
            var availableDiscounts = discounts.Select(d=>d.ApplyDiscounts(shoppingBasket));
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
            var policy = StorePredicatesManager.Instance.GetPolicy(Id);
            return policy.Check(shoppingBasket);
        }
        //TODO
        public void SetPolicy(string userID, Policy policy)
        {
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return;
            StorePredicatesManager.Instance.SetPolicy(Id, policy);
        }

        public Policy GetPolicy()
        {
            var policy = StorePredicatesManager.Instance.GetPolicy(Id);
            return policy;
        }

        //TODO
        public Guid AddRule(string userID, IRule rule)
        {
            var policy = StorePredicatesManager.Instance.GetPolicy(Id);
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return new Guid();
            policy.AddRule(rule);
            return rule.GetId();
        }

        //TODO
        public void RemoveRule(string userID)
        {
            var policy = StorePredicatesManager.Instance.GetPolicy(Id);
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return;
            policy.RemoveRule();
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public async Task<string> AddProduct(Product product, string userID)
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
                
            }
            await MarketStores.Instance.addToCategory(product, product.Category);
            await ProxyMarketContext.Instance.saveChanges();
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
        public async Task<string> EditProduct(Guid productID, Product editedProduct, string userID)
        {
            Product prev;
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()))
                return "Invalid user";
            if (!hasPremssion(userID, Permission.EditProduct))
                return "No permission";
            
                if (!_products.Where(p => p.Id.Equals(productID)).Any())
                    return "Product not in the store";
                if (!validProduct(editedProduct))
                    return "Invalid edit";
                prev= _products.Where(p => p.Id.Equals(productID)).First();
                if (!prev.category.Equals(editedProduct.category))
                {
                    MarketStores.Instance.removeFromCategory(prev, prev.Category);
                    await MarketStores.Instance.addToCategory(prev, editedProduct.Category);
                }
                lock (_products)
                {
                prev.category = editedProduct.category;
                prev.Name = editedProduct.Name;
                prev.Price = editedProduct.Price;
                prev.Quantity = editedProduct.Quantity;
                prev.Rating = editedProduct.rating;
                prev.Weight = editedProduct.Weight;
            }
            await ProxyMarketContext.Instance.saveChanges();
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
            try
            {
                if (type.Equals("owner"))
                {
                    await a.AddAppointmentOwner(assignee, this);
                    PublisherManagement.Instance.EventNotification(assigneeID, EventType.AddAppointmentEvent, assigner.Username + " has appointed you as an owner for store " + name);
                }
                else
                {
                    await a.AddAppointmentManager(assignee, this);
                    PublisherManagement.Instance.EventNotification(assigneeID, EventType.AddAppointmentEvent, assigner.Username + " has appointed you as a manager for store "+name);
                }
                ret = "Success";
                        
            }
            catch { ret = "this member is already assigned as a store owner or manager"; }
            
            return ret;
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public async Task<string> DefineManagerPermissionsAsync(string managerID, string assignerID, List<Permission> newPermissions)
        {
            Manager manager;
            if (!managers.Where(m=>m.username.Equals(managerID)).Any())
                return "Manager doesn't exist";
            manager = managers.Where(m => m.username.Equals(managerID)).First();
            Appointer a;
            if (managers.Where(m => m.username.Equals(assignerID)).Any())
                return "Invalid assigner";
            else if (founder.Username.Equals(assignerID))
                a = founder;
            else if (owners.Where(p => p.username.Equals(assignerID)).Any())
                a = owners.Where(p => p.username.Equals(assignerID)).First();
            else
                return "Invalid assigner";
            try
            {
                await a.DefinePermissions(managerID, manager,newPermissions);
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
        public async Task<string> RemoveOwnerAsync(String ownerName, String assigner)
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
                    ret = await RemoveOwnerAsync(appointee, ownerName);
                    if (!ret.Equals("success"))
                        return ret;
                }
            }
            ownerToRemove.removeManagers();
            lock (this)
            {
                ownerToRemove.removePermission(this);
                owners.Remove(ownerToRemove);
               
            }
            await MarketDAL.Instance.removeOwner(ownerToRemove);
            ret = "success";
            PublisherManagement.Instance.EventNotification(ownerName, EventType.RemoveAppointment, assigner + " has revoked your appointment as an owner for store " + name);
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
            var discounts = StorePredicatesManager.Instance.GetDiscounts(Id);
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            discounts.Add(discount);
            return discount.Id;
        }

        //TODO
        public Guid RemoveDiscount(string userID, Guid discountId)
        {
            var discounts = StorePredicatesManager.Instance.GetDiscounts(Id);
            //Have no permission to do the action
            if ((!founder.Username.Equals(userID)) && !(owners.Where(o => o.username.Equals(userID)).Any()) && !(managers.Where(m => m.username.Equals(userID)).Any()) || !hasPremssion(userID, Permission.EditDiscount))
                return new Guid();
            Discount d = GetDiscountById(discountId);
            if (d != null)
            {    //remove old discount if exists
                discounts.Remove(d);
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
            var discounts = StorePredicatesManager.Instance.GetDiscounts(Id);
            IEnumerable<Discount> uniquediscounts = discounts.Where(discount => discount.Id.Equals(discountId));
            return uniquediscounts.FirstOrDefault();
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

        public bool CheckPermission(string username, Permission permission)
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
            var policy = StorePredicatesManager.Instance.GetPolicy(Id);

            if (policy.Rule.GetId().Equals(ruleId))
            {
                return policy.Rule;
            }
            return null;
        }
        //TODO
        public IRule GetDiscountRuleById(Guid ruleId)
        {
            var discounts = StorePredicatesManager.Instance.GetDiscounts(Id);
            return discounts.Where(d => d.GetRule().GetId().Equals(ruleId)).FirstOrDefault().GetRule();
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

        public bool Contains(String comparator, String list)
        {
            switch (list)
            {
                case "Owners":
                    return Owners.Where(o => o.Username.Equals(comparator)).Any();
                case "Managers":
                    return Managers.Where(m => m.username.Equals(comparator)).Any();
                case "Products":
                    return Products.Where(p => p.Id.Equals(comparator)).Any();
                default: return false;
            }
        }

        public Manager GetManager(String name)
        {
            foreach (Manager manager in Managers)
            {
                if (manager.username.Equals(name))
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

        public Appointer GetAppointer(string appointerName)
        {
            Appointer ret = GetOwner(appointerName);
            if (ret == null)
            {
                ret = founder;
            }
            return ret;
        }
    }
}
