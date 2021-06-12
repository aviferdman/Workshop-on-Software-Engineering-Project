using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.DAL;

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

        public async Task<ICollection<HistoryData>> GetUserHistory(string username)
        {
            ICollection<IHistory> history = await  marketUsers.GetUserHistory(username);
            ICollection<HistoryData> ret = new HashSet<HistoryData>();
            foreach (var h in history)
            {
                ret.Add(new HistoryData(h));
            }
            return ret;
        }

        public async Task<string> loginAsync(string usrname, string password, string guestusername)
        {
            return await marketUsers.AddMember(usrname, password, guestusername);
        }

        public Statistics getTodayStats()
        {
            return UsersDAL.Instance.getStatis(DateTime.Now.Date);
        }

        public Statistics getDatesStats(DateTime begin, DateTime end)
        {
            Statistics statistics = new Statistics();
            Statistics curr;
            while (begin.Day <= end.Day)
            {
                curr =UsersDAL.Instance.getStatis(begin.Date);
                statistics.adminNum += curr.adminNum;
                statistics.guestsNum += curr.guestsNum;
                statistics.managersNum += curr.managersNum;
                statistics.membersNum += curr.membersNum;
                statistics.ownersNum += curr.ownersNum;
                begin=begin.AddDays(1);
            }
            return statistics;
        }

        public async Task<string> logoutAsync(string usrname)
        {
            return await marketUsers.logout(usrname);
        }

        public async Task<ICollection<StoreData>> getUserStores(string usrname)
        {
            ICollection<Store> stores = await marketUsers.getUserStores(usrname);
            ICollection<StoreData> dataStores = new LinkedList<StoreData>();
            foreach (Store s in stores)
            {
                dataStores.Add(new StoreData(s));
            }
            return dataStores;
        }

        public Statistics GetStats()
        {
            return marketUsers.GetStats();
        }
    }
}
