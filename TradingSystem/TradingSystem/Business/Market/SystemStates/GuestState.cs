using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState() : base()
        {

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
            throw new UnauthorizedAccessException();
        }

    }
}
