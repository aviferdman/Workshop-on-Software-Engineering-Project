using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketUserService
    {
        private readonly MarketUsers marketUsers;

        public MarketUserService()
        {
            marketUsers = MarketUsers.Instance;
        }

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
