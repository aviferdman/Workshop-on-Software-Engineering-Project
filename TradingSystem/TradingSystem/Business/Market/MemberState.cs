using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        private Guid _userId;

        public MemberState(Guid userId) : base()
        {
            this._userId = userId;
        }

        public override History GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override History GetUserHistory(Guid userId)
        {
            return GetTransaction().GetHistory(_userId);
        }
    }
}
