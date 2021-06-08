using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class DiscountCompoundParamsDTO
    {
        [Required]
        public string? Username { get; set; } = "";
        public Guid StoreId { get; set; }

        [Required]
        public string? DiscountRuleRelation { get; set; } = "";
        public Guid DiscountId1 { get; set; }
        public Guid DiscountId2 { get; set; }
        public bool Decide { get; set; }
    }
}
