using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class AdministratorState : MemberState
    {
        private Guid _userId;
        private UserHistory _userHistory;
        public AdministratorState(Guid userId, UserHistory userHistory) : base(userId, userHistory)
        {
            _userId = userId;
            _userHistory = userHistory;
        }

        public override ICollection<IHistory> GetAllHistory()
        {
            return HistoryManager.Instance.GetAllHistories();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public override StoreHistory GetStoreHistory(Store store)
        {
            return store.GetStoreHistory(_userId);
        }

        //use case 40 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/66
        public override UserHistory GetUserHistory(Guid userId)
        {
            return _userHistory;
        }
    }
}
