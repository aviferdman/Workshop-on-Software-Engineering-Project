using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public class CustomerNegotiateBidParamsDTO
    {
        public Guid BidId { get; set; }
        public Guid StoreId { get; set; }
        public double NewPrice { get; set; }
    }
}
