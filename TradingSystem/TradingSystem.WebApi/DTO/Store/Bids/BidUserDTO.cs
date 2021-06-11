using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market.StorePackage;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public class BidUserDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid StoreId { get; set; }

        public double Price { get; set; }
        public BidStatus Status { get; set; }

        public static BidUserDTO FromBid(Bid bid)
        {
            return new BidUserDTO
            {
                Id = bid.Id,
                ProductId = bid.ProductId,
                StoreId = bid.StoreId,
                Price = bid.Price,
                Status = (BidStatus)bid.Status,
            };
        }
    }
}
