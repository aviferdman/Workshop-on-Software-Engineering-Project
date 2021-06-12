using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class DiscountParamsSimpleCreationDTO
    {
        [Required]
        public string Username { get; set; } = "";
        public Guid StoreId { get; set; }
        [Required]
        public DiscountParamsSimpleDTO Params { get; set; } = new DiscountParamsSimpleDTO();
    }
}
