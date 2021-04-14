using System;

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

        public HistoryData GetUserHistory(string username)
        {
            UserHistory history = marketUsers.GetUserHistory(username);
            return new HistoryData(history);
        }
    }
}
