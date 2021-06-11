using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.Business.Payment;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class MarketStores 
    {
        private static readonly string DEFAULT_ADMIN_USERNAME = "DEFAULT_ADMIN_USERNAME";
        private ConcurrentDictionary<Guid, Store> loadedStores;
        private HistoryManager historyManager;
        private static Transaction _transaction = Transaction.Instance;
        private MarketDAL dal;
        MarketUsers marketUsers;
        ProxyMarketContext proxy;

        public ConcurrentDictionary<Guid, Store> LoadedStores { get => loadedStores; set => loadedStores = value; }

        private MarketStores(ProxyMarketContext proxy ,HistoryManager historyManager, MarketDAL marketDAL, MarketUsers marketUsers)
        {
            this.proxy = proxy;
            dal = marketDAL;
            this.historyManager = historyManager;
            loadedStores = new ConcurrentDictionary<Guid, Store>();
            this.marketUsers = marketUsers;
        }

        public void tearDown()
        {
            loadedStores = new ConcurrentDictionary<Guid, Store>();
            dal.teardown();
        }

        public void ActivateDebugMode(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem, bool debugMode)
        {
            _transaction.ActivateDebugMode(deliverySystem, paymentSystem, debugMode);
        }

        //USER FUNCTIONALITY

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public async Task<Store> CreateStore(string name, string username, CreditCard bank, Address address)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(CreateStore));
            User user = marketUsers.GetUserByUserName(username);
            if (user == null || typeof(GuestState).IsInstanceOfType(user.State))
                return null;
            Store store = new Store(name, bank, address);
            store.SetFounder(Founder.makeFounder((MemberState)user.State, store));
            if (!loadedStores.TryAdd(store.Id, store))
                return null;
            await dal.AddStore(store);
            //PublisherManagement.Instance.EventNotification(username, EventType.OpenStoreEvent, "Opened Store");

            return store;
        }

        public async Task<Result<WorkerDetails>> GetPerms(Guid storeID, string username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetInfoSpecific));
            Store store = await GetStoreById(storeID);
            if (store == null)
                return null;
            return store.GetPerms(username);
        }



        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public async Task<ICollection<Store>> GetStoresByName(string name)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetStoresByName));
            ICollection<Store> stores = await dal.GetStoresByName(name.ToLower());
            return stores;
        }

        public void DeleteAll()
        {
            loadedStores = new ConcurrentDictionary<Guid, Store>();
            dal.teardown();
        }


        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public async Task<ICollection<IHistory>> GetStoreHistory(string username, Guid storeId)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetStoreHistory));
            User user = marketUsers.GetUserByUserName(username);
            Store store = await GetStoreById(storeId);
            return await store.GetStoreHistory(username);
        }

        public async Task<Store> GetStoreById(Guid storeId)
        {
            Store s = null;
            if(!loadedStores.TryGetValue(storeId, out s))
            {
                return await dal.getStore(storeId);
            }
            return s;
        }

        public void findStoreProduct(out Store found, out Product p, Guid pid)
        {
            p = null;
            found = null;
            foreach (Store s in loadedStores.Values)
            {
                if (s.Products.Where(p=> p.Id.Equals(pid)).Any())
                {
                    p = s.Products.Where(p => p.Id.Equals(pid)).First();
                    found = s;
                    return;
                }
            }
            dal.findStoreProduct(out found, out  p,  pid);
            if(found!=null)
                loadedStores.TryAdd(found.Id, found);

        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public async Task<Result<Product>> AddProduct(ProductData productData, Guid storeID, String username)
        {
            Store store = await GetStoreById(storeID);
            String res = "error";
            if (store == null)
                return new Result<Product>(null, true, "Store doesn't exist");
            productData.StoreName = store.name;
            Product product = new Product(productData);
            try
            {
                res = await store.AddProduct(product, username);
            }
            catch (Exception e)
            {
                return new Result<Product>(product, true, res);
            }
            if (res.Equals("Product added"))
                return new Result<Product>(product, false, res);
            return new Result<Product>(product, true, res);
        }
        //TODO
        public async  Task<Result<bool>> CustomerCreateBid(string username, Guid storeId, Guid productId, double newBidPrice)
        {
            User u = marketUsers.GetUserByUserName(username);
            Store store = await GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            foreach (var owner in store.GetOwners())
            {
                var ownerUsername = owner.username;
                var message = $"{username} {storeId} {productId} {newBidPrice}";
                PublisherManagement.Instance.EventNotification(ownerUsername, EventType.RequestPurchaseEvent, message);
            }
            return new Result<bool>(true, false, "");

        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public async Task<String> RemoveProduct(Guid productID, Guid storeID, String username)
        {
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return store.RemoveProduct(productID, username);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public async Task<String> EditProduct(Guid productID, ProductData details, Guid storeID, String username)
        {
            Product editedProduct = new Product(details);
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return await store.EditProduct(productID, editedProduct, username);
        }


        //functional requirement 4.3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/47
        public async Task<String> makeOwner(String assigneeName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(makeOwner));
            return await AssignMember(assigneeName, storeID, assignerName, "owner");
        }

        //functional requirement 4.5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/55
        public async Task<String> makeManager(String assigneeName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(makeManager));
            return await AssignMember(assigneeName, storeID, assignerName, "manager");
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public async Task<String> DefineManagerPermissions(String managerName, Guid storeID, String assignerName, List<Permission> permissions)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(DefineManagerPermissions));
            User assigner = marketUsers.GetUserByUserName(assignerName);
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return await store.DefineManagerPermissionsAsync(managerName, assignerName, permissions);
        }

        public async Task<String> AssignMember(String assigneeName, Guid storeID, String assignerName, string type)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(AssignMember));
            User assigner = marketUsers.GetUserByUserName(assignerName);
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return await store.AssignMember(assigneeName, assigner, type);
        }

        //functional requirement 4.7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/57
        public async Task<String> RemoveManager(String managerName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(RemoveManager));
            User assigner = marketUsers.GetUserByUserName(assignerName);
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return store.RemoveManager(managerName, assignerName);
        }

        //functional requirement 4.4 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/136
        public async Task<String> RemoveOwner(String ownerName, Guid storeID, String assignerName)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(RemoveOwner));
            User assigner = marketUsers.GetUserByUserName(assignerName);
            Store store = await GetStoreById(storeID);
            if (store == null)
                return "Store doesn't exist";
            return await store.RemoveOwnerAsync(ownerName, assignerName);
        }

        public async  Task addToCategory(Product p, string category)
        {
            Category cat=await dal.AddNewCategory(category);
            cat.addProduct(p);
            await proxy.saveChanges();
        }

        public async void removeFromCategory(Product p, string category)
        {
            Category cat = await dal.AddNewCategory(category);
            cat.Products.Remove(p);
            await proxy.saveChanges();

        }



        //use case 4 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/52
        public async Task<ICollection<Product>> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            
            return await dal.findProducts(keyword,price_range_low,price_range_high,rating, category);
        }

        //functional requirement 4.9 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/60
        public async Task<List<WorkerDetails>> GetInfo(Guid storeID, String username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetInfo));
            Store store = await GetStoreById(storeID);
            if (store == null)
                return new List<WorkerDetails>(); ;
            return store.GetInfo(username);
        }

        public async Task<WorkerDetails> GetInfoSpecific(Guid storeID, String workerName, String username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketStores) + " " + nameof(GetInfoSpecific));
            Store store = await GetStoreById(storeID);
            if (store == null)
                return null;
            return store.GetInfoSpecific(workerName, username);
        }
    }
}
