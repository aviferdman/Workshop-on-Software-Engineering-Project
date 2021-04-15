using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class AdministratorState : MemberState
    {
        private string _userId;
        private ICollection<IHistory> _userHistory;
        public AdministratorState(string username, ICollection<IHistory> userHistory) : base(username, userHistory)
        {
            _userId = username;
            _userHistory = userHistory;
        }

        public override ICollection<IHistory> GetAllHistory()
        {
            return HistoryManager.Instance.GetAllHistories();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public override ICollection<IHistory> GetStoreHistory(Store store)
        {
            return store.GetStoreHistory(_userId);
        }

        //use case 40 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/66
        public override ICollection<IHistory> GetUserHistory(string username)
        {
            return _userHistory;
        }
    }
}
