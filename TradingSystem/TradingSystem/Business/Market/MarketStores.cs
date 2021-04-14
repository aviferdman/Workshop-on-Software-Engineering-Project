using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class MarketStores: IMarketStores
    {
        private static readonly string DEFAULT_ADMIN_USERNAME = "DEFAULT_ADMIN_USERNAME";

        private ConcurrentDictionary<Guid,IStore> _stores;
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
        }

        public void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode)
        {
            _transaction.ActivateDebugMode(deliveryAdapter, paymentAdapter, debugMode);
        }

        //USER FUNCTIONALITY

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public Store CreateStore(string name, string username, BankAccount bank, Address address)
        {
            User user = MarketUsers.Instance.GetUserByUserName(username);
            if (typeof(GuestState).IsInstanceOfType(user.State))
                return null;
            Store store = new Store(name, bank, address);
            store.Personnel.TryAdd(user.Id, new Founder(user.Id));
            if (!_stores.TryAdd(store.Id, store))
                return null;
            return store;
        }

        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public ICollection<Store> GetStoresByName(string name)
        {
            LinkedList<Store> stores = new LinkedList<Store>();
            foreach(Store s in _stores.Values)
            {
                if (s.Name.Equals(name))
                {
                    stores.AddLast(s);
                }
            }
            return stores;
        }


        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public StoreHistory GetStoreHistory(string username, Guid storeId)
        {
            User user = MarketUsers.Instance.GetUserByUserName(username);
            IStore store = GetStoreById(storeId);
            return store.GetStoreHistory(user.Id);
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public double ApplyDiscounts(string username, Guid storeId)
        {
            IStore store = GetStoreById(storeId);
            User user = MarketUsers.Instance.GetUserByUserName(username);
            return store.ApplyDiscounts(user.ShoppingCart.GetShoppingBasket(store));
        }


        public IStore GetStoreById(Guid storeId)
        {
            IStore s=null;
            _stores.TryGetValue(storeId, out s);
            return  s;
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String AddProduct(ProductData productData, Guid storeID, String username)
        {
            Product product = new Product(productData);
            User user = MarketUsers.Instance.GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AddProduct(product, user.Id);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            User user = MarketUsers.Instance.GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.RemoveProduct(productName, user.Id);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            Product editedProduct = new Product(details);
            User user = MarketUsers.Instance.GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.EditProduct(productName, editedProduct, user.Id);
        }


        //functional requirement 4.3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/47
        public String makeOwner(String assigneeName, Guid storeID, String assignerName)
        {
            return AssignMember(assigneeName, storeID, assignerName, AppointmentType.Owner);
        }

        //functional requirement 4.5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/55
        public String makeManager(String assigneeName, Guid storeID, String assignerName)
        {
            return AssignMember(assigneeName, storeID, assignerName, AppointmentType.Manager);
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(String managerName, Guid storeID, String assignerName, List<Permission> permissions)
        {
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            IStore store;
            Guid managerID;
            try
            {
                managerID = UserManagement.UserManagement.Instance.getIdByUsername(managerName);
            }catch { return "Manager doesn't exist"; }
            Guid assignerID;
            try
            {
                assignerID = UserManagement.UserManagement.Instance.getIdByUsername(assignerName);
            }
            catch { return "Manager doesn't exist"; }
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.DefineManagerPermissions(managerID, assignerID, permissions);
        }

        public String AssignMember(String assigneeName, Guid storeID, String assignerName, AppointmentType type)
        {
            Guid assigneeID;
            try
            {
                assigneeID = UserManagement.UserManagement.Instance.getIdByUsername(assigneeName);
            }
            catch { return "Assignee is not a member"; }
            User assigner = MarketUsers.Instance.GetUserByUserName(assignerName);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AssignMember(assigneeID, assigner, type);
        }

       
        public ICollection<Product> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            List<Product> products = new List<Product>();
            foreach (Store s in _stores.Values)
            {
                products.AddRange(s.findProducts(keyword, price_range_low, price_range_high, rating, category));
            }
            return products;
        }

    }
}
