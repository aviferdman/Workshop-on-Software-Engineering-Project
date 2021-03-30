using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Market
    {
        private ConcurrentBag<Store> _stores;
        private ConcurrentDictionary<string, User> activeUsers;
        private static readonly Lazy<Market>
        _lazy =
        new Lazy<Market>
            (() => new Market());

        public static Market Instance { get { return _lazy.Value; } }

        private Market()
        {
            _stores = new ConcurrentBag<Store>();
            activeUsers = new ConcurrentDictionary<string, User>();
        }

        //USER FUNCTIONALITY

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public Store CreateStore(string name, string username, BankAccount bank, Address address)
        {
            User user = GetUserByUserName(username);
            Store store = user.CreateStore(name, bank, address);
            _stores.Add(store);
            return store;
            
        }

        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public ConcurrentBag<Store> GetStoresByName(string name)
        {
            ConcurrentBag<Store> stores = new ConcurrentBag<Store>();
            foreach(Store s in _stores)
            {
                if (s.Name.Equals(name))
                {
                    stores.Add(s);
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
            return user.GetAllHistory();
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
            return user.GetStoreHistory(storeId);
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public double ApplyDiscounts(string username, Guid storeId)
        {
            Store store = GetStoreById(storeId);
            User user = GetUserByUserName(username);
            return store.ApplyDiscounts(user.ShoppingCart.GetShoppingBasket(store));
        }

        public bool AddSubject(string username, string subjectUsername, Guid storeId, Permission permission)
        {
            User user = GetUserByUserName(username);
            User subject = GetUserByUserName(subjectUsername);
            return user.AddSubject(storeId, permission, subject.StorePermission);
        }

        public bool RemoveSubject(string username, string subjectUsername, Guid storeId)
        {
            User user = GetUserByUserName(username);
            User subject = GetUserByUserName(subjectUsername);
            return user.RemoveSubject(storeId, subject.StorePermission);
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
            var possibleStores = _stores.Where(s => s.Id.Equals(storeId));
            return possibleStores.FirstOrDefault();
        }

    }
}
