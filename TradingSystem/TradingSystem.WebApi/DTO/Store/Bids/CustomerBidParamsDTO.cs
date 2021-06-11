using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public class CustomerBidParamsDTO
    {
        public Guid BidId { get; set; }
        public Guid StoreId { get; set; }
    }
}
