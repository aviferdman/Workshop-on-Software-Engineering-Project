using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market.StorePackage;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public class BidStoreDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Username { get; set; }

        public double Price { get; set; }
        public BidStatus Status { get; set; }
        public bool ApprovedByMe { get; set; }

        public static BidStoreDTO FromBid(Bid bid)
        {
            return new BidStoreDTO
            {
                Id = bid.Id,
                ProductId = bid.ProductId,
                Username = bid.Username,
                Price = bid.Price,
                Status = (BidStatus)bid.Status,
            };
        }
    }
}
