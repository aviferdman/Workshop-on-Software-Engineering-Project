using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        private string _userId;
        private ICollection<IHistory> _userHistory;

        public MemberState(string userId, ICollection<IHistory> userHistory) : base()
        {
            this._userId = userId;
            this._userHistory = userHistory;
        }

        public override ICollection<IHistory> GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override ICollection<IHistory> GetStoreHistory(Store store)
        {
            throw new UnauthorizedAccessException();
        }

        public override ICollection<IHistory> GetUserHistory(Guid userId)
        {
            return _userHistory;
        }
    }
}
