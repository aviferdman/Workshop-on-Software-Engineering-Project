using System;

namespace TradingSystem.Business.Market.StoreStates
{
    public class BidAcceptence
    {
        public Guid id { get; set; }
        public bool accept { get; set; }

        public BidAcceptence(Guid bidId)
        {
            this.id = bidId;
            this.accept = false;
        }

        public BidAcceptence()
        {
        }
    }
}