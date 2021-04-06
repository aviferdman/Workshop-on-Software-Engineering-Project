using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState() : base()
        {

        }

        public override History GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override History GetUserHistory(Guid userId)
        {
            throw new UnauthorizedAccessException();
        }

    }
}
