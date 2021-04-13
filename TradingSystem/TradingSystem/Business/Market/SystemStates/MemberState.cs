using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        private Guid _userId;
        private UserHistory _userHistory;

        public MemberState(Guid userId, UserHistory userHistory) : base()
        {
            this._userId = userId;
            this._userHistory = userHistory;
        }

        public override ICollection<IHistory> GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override StoreHistory GetStoreHistory(Store store)
        {
            throw new UnauthorizedAccessException();
        }

        public override UserHistory GetUserHistory(Guid userId)
        {
            return _userHistory;
        }
    }
}
