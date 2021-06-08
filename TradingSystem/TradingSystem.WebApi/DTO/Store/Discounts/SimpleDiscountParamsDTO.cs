using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class SimpleDiscountParamsDTO
    {
        [Required]
        public string Username { get; set; } = "";
        public Guid StoreId { get; set; }
        [Required]
        public string DiscountType { get; set; } = "";
        public double Percent { get; set; }
        public string? Category { get; set; }
        public Guid? ProductId { get; set; }
    }
}
