using System;

namespace TradingSystem.Business.Market.StoreStates
{
    public class BidAcceptence
    {
        public Guid bidId { get; set; }
        public bool accept { get; set; }

        public BidAcceptence(Guid bidId)
        {
            this.bidId = bidId;
            this.accept = false;
        }
    }
}