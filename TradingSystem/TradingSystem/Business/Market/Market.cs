using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingSystem.Business.Market
{
    public class Market: IMarket
    {
        private ConcurrentDictionary<Guid,Store> _stores;
        private ConcurrentDictionary<string, User> activeUsers;
        private static readonly Lazy<Market>
        _lazy =
        new Lazy<Market>
            (() => new Market());

        public static Market Instance { get { return _lazy.Value; } }

        private Market()
        {
            _stores = new ConcurrentDictionary<Guid, Store>();
            activeUsers = new ConcurrentDictionary<string, User>();
        }

        //functional requirement 2.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/10
        public string AddGuest()
        {
            User u = new User();
            string GuidString;
            do
            {
                Guid g = Guid.NewGuid();
                GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                u.Username=GuidString;
            } while (activeUsers.TryAdd(GuidString, u));
            return u.Username;
        }

        //functional requirement 2.2: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/13
        public void RemoveGuest(String usrname)
        {
            User guest = null;
            activeUsers.TryRemove(usrname, out guest);
        }


        ///adding member to market after register - <see cref="UserManagement.UserManagement.SignUp(string, string, Address, string)"/> 
        public bool AddMember(String usrname)
        {
            User u = new User(usrname);
            User guest = null;
            string GuidString;
            u.State = new MemberState(u.Id);
            while (!activeUsers.TryAdd(usrname, u))
            {
                if(activeUsers.TryRemove(usrname,out guest))
                {
                    do
                    {
                        Guid g = Guid.NewGuid();
                        GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");
                        u.Username = GuidString;
                    } while (activeUsers.TryAdd(GuidString, u));
                }
                
            };
            return true;
        }

        ///after login- <see cref="UserManagement.UserManagement.LogIn(string, string, string)"/> mark that user is logged in

        public bool login(String usrname)
        {
            User mem = null;
            if(activeUsers.TryGetValue(usrname, out mem))
            {
                mem.IsLoggedIn = true;
                return true;
            }
            return false;
        }

            //USER FUNCTIONALITY

            //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public Store CreateStore(string name, string username, BankAccount bank, Address address)
        {
            User user = GetUserByUserName(username);
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

        //use case 11 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address)
        {
            User user = GetUserByUserName(username);
            return user.PurchaseShoppingCart(bank, phone, address);
        }

        //use case 39 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/65
        public History GetAllHistory(string username)
        {
            User user = GetUserByUserName(username);
            return user.State.GetAllHistory();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public History GetUserHistory(string username)
        {
            User user = GetUserByUserName(username);
            Guid userId = user.Id;
            return user.GetUserHistory(userId);
        }

        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public History GetStoreHistory(string username, Guid storeId)
        {
            User user = GetUserByUserName(username);
            return user.State.GetStoreHistory(storeId);
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public double ApplyDiscounts(string username, Guid storeId)
        {
            Store store = GetStoreById(storeId);
            User user = GetUserByUserName(username);
            return store.ApplyDiscounts(user.ShoppingCart.GetShoppingBasket(store));
        }

        public bool AddPerssonel(string username, string subjectUsername, Guid storeId, AppointmentType permission)
        {
            User u = GetUserByUserName(username);
            User newUser= GetUserByUserName(subjectUsername);
            Store store ;
            if (!_stores.TryGetValue(storeId, out store))
                return false;
            return store.AddPerssonel(u.Id,newUser.Id,permission);
        }

        public bool RemovePerssonel(string username, string subjectUsername, Guid storeId)
        {
            User u = GetUserByUserName(username);
            User newUser = GetUserByUserName(subjectUsername);
            Store store;
            if (!_stores.TryGetValue(storeId, out store))
                return false;
            return store.RemovePerssonel(u.Id, newUser.Id);
        }

        private User GetUserByUserName(string username)
        {
            User u = null;
            if (!activeUsers.TryGetValue(username, out u))
            {
                return null;
            }
            return u;
        }

        private Store GetStoreById(Guid storeId)
        {
            Store s=null;
            _stores.TryGetValue(storeId, out s);
            return  s;
        }

        public String AddProduct(ProductData productData, Guid storeID, String username)
        {
            Product product = new Product(productData);
            User user = GetUserByUserName(username);
            Store store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AddProduct(product, user.Id);
        }

        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            User user = GetUserByUserName(username);
            Store store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.RemoveProduct(productName, user.Id);
        }

        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            Product editedProduct = new Product(details);
            User user = GetUserByUserName(username);
            Store store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.EditProduct(productName, editedProduct, user.Id);
        }

    }
}
