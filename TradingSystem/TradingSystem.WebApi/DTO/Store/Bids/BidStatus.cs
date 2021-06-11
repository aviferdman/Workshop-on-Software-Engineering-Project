using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public enum BidStatus
    {
        Accept,
        Deny,
        CustomerNegotiate,
        OwnerNegotiate
    }
}
