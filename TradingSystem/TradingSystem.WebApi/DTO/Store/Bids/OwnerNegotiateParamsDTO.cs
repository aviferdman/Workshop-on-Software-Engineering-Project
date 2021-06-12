using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public class OwnerNegotiateParamsDTO
    {
        [Required]
        public string Username { get; set; } = "";
        public Guid BidId { get; set; }
        public Guid StoreId { get; set; }
        public double NewPrice { get; set; }
    }
}
