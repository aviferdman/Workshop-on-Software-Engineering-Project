using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Market
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

        //to continue
        public Store CreateStore(string name, string username, BankAccount bank)
        {
            User u = null;
            if(!activeUsers.TryGetValue(username, out u))
            {
                return null;
            }
            return null;
            
        }

        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public ConcurrentBag<Store> GetStoresByName(string name)
        {
            ConcurrentBag<Store> stores= new ConcurrentBag<Store>();
            foreach(Store s in _stores)
            {
                if (s.Name.Equals(name))
                {
                    stores.Add(s);
                }
            }
            return stores;
        }
    }
}
