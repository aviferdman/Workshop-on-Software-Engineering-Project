using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState() : base("")
        {

        }

        public override void AddHistory(IHistory history)
        {
            
        }

        public override Task<ICollection<IHistory>> GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override Task<ICollection<IHistory>> GetStoreHistory(Store store)
        {
            throw new UnauthorizedAccessException();
        }

        public override Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            throw new UnauthorizedAccessException();
        }

    }
}
