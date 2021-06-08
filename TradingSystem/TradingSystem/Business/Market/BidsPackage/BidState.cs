using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;

namespace TradingSystem.Business.Market.BidsPackage
{
    public class BidState
    {
        public Guid id { get; set; }
        private ICollection<Prem> _ownersAccepted;
        private Bid _bid;

        public ICollection<Prem> OwnersAccepted { get => _ownersAccepted; set => _ownersAccepted = value; }
        public Bid Bid { get => _bid; set => _bid = value; }

        public BidState(Bid bid)
        {
            this._bid = bid;
            this.OwnersAccepted = new HashSet<Prem>();
        }

        public BidState()
        {
        }

        public async Task AddAcceptence(string ownerUsername)
        {
            if (!OwnersAccepted.Where(p=>p.p.Equals(ownerUsername)).Any())
            {
                this.OwnersAccepted.Add(new Prem(ownerUsername));
            }
        }

        public async Task RemoveAcceptence(string ownerUsername)
        {
            if (OwnersAccepted.Where(p => p.p.Equals(ownerUsername)).Any())
            {
                this.OwnersAccepted.Remove(OwnersAccepted.Single(p => p.p.Equals(ownerUsername)));
            }
        }
    }
}
