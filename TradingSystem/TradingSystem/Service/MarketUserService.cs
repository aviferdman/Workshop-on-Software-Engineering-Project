using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketUserService
    {
        private static readonly Lazy<MarketUserService> instanceLazy = new Lazy<MarketUserService>(() => new MarketUserService(), true);

        private readonly MarketUsers marketUsers;

        private MarketUserService()
        {
            marketUsers = MarketUsers.Instance;
        }

        public static MarketUserService Instance => instanceLazy.Value;

        //returns uniqe username for guest
        public string AddGuest()
        {
            return marketUsers.AddGuest();
        }

        public void RemoveGuest(string username)
        {
            marketUsers.RemoveGuest(username);
        }

        public ICollection<HistoryData> GetUserHistory(string username)
        {
            ICollection<IHistory> history = marketUsers.GetUserHistory(username);
            ICollection<HistoryData> ret = new HashSet<HistoryData>();
            foreach (var h in history)
            {
                ret.Add(new HistoryData(h));
            }
            return ret;
        }

        public string login(string usrname, string password, string guestusername)
        {
            return marketUsers.AddMember(usrname, password, guestusername);
        }

        public string logout(string usrname)
        {
            return marketUsers.logout(usrname);
        }

        public ICollection<StoreData> getUserStores(string usrname)
        {
            ICollection<Store> stores = marketUsers.getUserStores(usrname);
            ICollection<StoreData> dataStores = new LinkedList<StoreData>();
            foreach (Store s in stores)
            {
                dataStores.Add(new StoreData(s));
            }
            return dataStores;
        }
    }
}
