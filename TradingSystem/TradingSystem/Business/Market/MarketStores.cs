using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Payment;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class MarketStores : IMarketStores
    {
        private ConcurrentDictionary<Guid, IStore> _stores;
        private ConcurrentDictionary<string, Category> categories;
        private HistoryManager historyManager;
        private static Transaction _transaction = Transaction.Instance;
        private static readonly Lazy<MarketStores>
        _lazy =
        new Lazy<MarketStores>
            (() => new MarketStores());

        public static MarketStores Instance { get { return _lazy.Value; } }

        public ConcurrentDictionary<Guid, IStore> Stores { get => _stores; set => _stores = value; }

        private MarketStores()
        {
            _stores = new ConcurrentDictionary<Guid, IStore>();

            historyManager = HistoryManager.Instance;
            categories = new ConcurrentDictionary<string, Category>();
        }

        public void tearDown()
        {
            _stores = new ConcurrentDictionary<Guid, IStore>();

            historyManager = HistoryManager.Instance;
            categories = new ConcurrentDictionary<string, Category>();
        }

        public void ActivateDebugMode(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem, bool debugMode)
        {
            _transaction.ActivateDebugMode(deliverySystem, paymentSystem, debugMode);
        }

        //USER FUNCTIONALITY

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public Store CreateStore(string name, string username, CreditCard bank, Address address)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(CreateStore));
            User user = MarketUsers.Instance.GetUserByUserName(username);
            if (user == null || typeof(GuestState).IsInstanceOfType(user.State))
                return null;
            Store store = new Store(name, bank, address);
            store.Founder = Founder.makeFounder((MemberState)user.State, store);
            if (!_stores.TryAdd(store.Id, store))
                return null;
            PublisherManagement.Instance.EventNotification(username, EventType.OpenStoreEvent, ConfigurationManager.AppSettings["OpenedStoreMessage"]);

            return store;
        }

        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public ICollection<Store> GetStoresByName(string name)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetStoresByName));
            LinkedList<Store> stores = new LinkedList<Store>();
            foreach (Store s in _stores.Values)
            {
                if (s.Name.Equals(name))
                {
                    stores.AddLast(s);
                }
            }
            return stores;
        }

        public void DeleteAll()
        {
            _stores = new ConcurrentDictionary<Guid, IStore>();

            historyManager = HistoryManager.Instance;
            categories = new ConcurrentDictionary<string, Category>();
        }


        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public ICollection<IHistory> GetStoreHistory(string username, Guid storeId)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetStoreHistory));
            User user = MarketUsers.Instance.GetUserByUserName(username);
            IStore store = GetStoreById(storeId);
            return store.GetStoreHistory(username);
        }

        public Result<bool> OwnerAcceptBid(string ownerUsername, string username, Guid storeId, Guid productId, double newBidPrice)
        {
            IStore store = null;
            User u = MarketUsers.Instance.GetUserByUserName(username);
            if (!_stores.TryGetValue(storeId, out store))
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            return store.AcceptBid(ownerUsername, username, productId, newBidPrice);
        }

        public IStore GetStoreById(Guid storeId)
        {
            IStore s = null;
            _stores.TryGetValue(storeId, out s);
            return s;
        }

        public void findStoreProduct(out Store found, out Product p, Guid pid)
        {
            p = null;
            found = null;
            foreach (Store s in _stores.Values)
            {
                if (s.Products.TryGetValue(pid, out p))
                {
                    found = s;
                    break;
                }

            }
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public Result<Product> AddProduct(ProductData productData, Guid storeID, String username)
        {
            Product product = new Product(productData);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return new Result<Product>(product, true, "Store doesn't exist");
            String res = store.AddProduct(product, username);
            if (res.Equals("Product added"))
                return new Result<Product>(product, false, res);
            return new Result<Product>(product, true, res);
        }

        public Result<bool> CustomerRequestBid(string username, Guid storeId, Guid productId, double newBidPrice)
        {
            IStore store = null;
            User u = MarketUsers.Instance.GetUserByUserName(username);
            if (!_stores.TryGetValue(storeId, out store))
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            foreach (var owner in store.GetOwners())
            {
                var ownerUsername = owner.Key;
                var message = $"{username} {storeId} {productId} {newBidPrice}";
                PublisherManagement.Instance.EventNotification(ownerUsername, EventType.RequestPurchaseEvent, message);
            }
            return new Result<bool>(true, false, "");

        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(Guid productID, Guid storeID, String username)
        {
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.RemoveProduct(productID, username);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(Guid productID, ProductData details, Guid storeID, String username)
        {
            Product editedProduct = new Product(details);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.EditProduct(productID, editedProduct, username);
        }


        //functional requirement 4.3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/47
        public String makeOwner(String assigneeName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(makeOwner));
            return AssignMember(assigneeName, storeID, assignerName, "owner");
        }

        //functional requirement 4.5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/55
        public String makeManager(String assigneeName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(makeManager));
            return AssignMember(assigneeName, storeID, assignerName, "manager");
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(String managerName, Guid storeID, String assignerName, List<Permission> permissions)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(DefineManagerPermissions));
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.DefineManagerPermissions(managerName, assignerName, permissions);
        }

        public String AssignMember(String assigneeName, Guid storeID, String assignerName, string type)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(AssignMember));
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AssignMember(assigneeName, assigner, type);
        }

        //functional requirement 4.7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/57
        public String RemoveManager(String managerName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(RemoveManager));
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            if (!_stores.TryGetValue(storeID, out IStore store))
                return "Store doesn't exist";
            return store.RemoveManager(managerName, assignerName);
        }

        //functional requirement 4.4 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/136
        public String RemoveOwner(String ownerName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(RemoveOwner));
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            if (!_stores.TryGetValue(storeID, out IStore store))
                return "Store doesn't exist";
            return store.RemoveOwner(ownerName, assignerName);
        }

        public void addToCategory(Product p, string category)
        {
            Category cat;
            if (categories.TryGetValue(category, out cat))
            {
                cat.addProduct(p);
            }
            else
            {
                cat = new Category(category);
                cat.addProduct(p);
                if (!categories.TryAdd(category, cat))
                    addToCategory(p, category);
            }
        }

        public void removeFromCategory(Product p, string category)
        {
            Category cat;
            if (categories.TryGetValue(category, out cat))
            {
                if (cat.Products.Contains(p))
                    cat.Products.Remove(p);
            }

        }



        //use case 4 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/52
        public ICollection<Product> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            List<Product> products = new List<Product>();
            Category cat;
            if (category != null)
            {
                if (categories.TryGetValue(category, out cat))
                {
                    return cat.getAllProducts(keyword, price_range_low, price_range_high, rating);
                }
                else
                    return products;
            }
            foreach (Category c in categories.Values)
            {
                products.AddRange(c.getAllProducts(keyword, price_range_low, price_range_high, rating));
            }
            return products;
        }

        //functional requirement 4.9 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/60
        public String getInfo(Guid storeID, String username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(getInfo));
            if (!_stores.TryGetValue(storeID, out IStore store))
                return "Store doesn't exist";
            return store.getInfo(username);
        }

        public String getInfoSpecific(Guid storeID, String workerName, String username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(getInfoSpecific));
            if (!_stores.TryGetValue(storeID, out IStore store))
                return "Store doesn't exist";
            return store.getInfoSpecific(workerName, username);
        }
    }
}
