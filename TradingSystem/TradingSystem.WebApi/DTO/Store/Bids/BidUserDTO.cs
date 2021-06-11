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
        public string? ProductName { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = "";

        public double Price { get; set; }
        public BidStatus_Api Status { get; set; }

        public static BidUserDTO FromBid(Bid bid)
        {
            return new BidUserDTO
            {
                Id = bid.Id,
                ProductId = bid.ProductId,
                StoreId = bid.StoreId,
                Price = bid.Price,
                Status = BidStatus_Api_Funcs.ConvertStatus(bid.Status),
            };
        }
    }
}
