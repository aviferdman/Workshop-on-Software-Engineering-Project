using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;

namespace TradingSystem.Business.Market.BidsPackage
{
    public class BidState
    {
        private ICollection<string> _ownersAccepted;
        private Bid _bid;

        public ICollection<string> OwnersAccepted { get => _ownersAccepted; set => _ownersAccepted = value; }
        public Bid Bid { get => _bid; set => _bid = value; }

        public BidState(Bid bid)
        {
            this._bid = bid;
            this.OwnersAccepted = new HashSet<string>();
        }

        public async Task AddAcceptence(string ownerUsername)
        {
            if (!OwnersAccepted.Contains(ownerUsername))
            {
                this.OwnersAccepted.Add(ownerUsername);
            }
        }

        public async Task RemoveAcceptence(string ownerUsername)
        {
            if (OwnersAccepted.Contains(ownerUsername))
            {
                this.OwnersAccepted.Remove(ownerUsername);
            }
        }
    }
}
