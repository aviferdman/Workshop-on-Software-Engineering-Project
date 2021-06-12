using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class DiscountParamsSimpleEditDTO
    {
        [Required]
        public string Username { get; set; } = "";
        public Guid StoreId { get; set; }
        public Guid DiscountId { get; set; }
        [Required]
        public DiscountParamsSimpleDTO Params { get; set; } = new DiscountParamsSimpleDTO();
    }
}
