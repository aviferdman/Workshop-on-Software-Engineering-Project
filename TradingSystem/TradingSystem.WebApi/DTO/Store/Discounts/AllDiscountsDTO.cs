using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class AllDiscountsDTO
    {
        public IEnumerable<DiscountDataDTO> LeafDiscounts { get; set; } = Enumerable.Empty<DiscountDataDTO>();
        public IEnumerable<DiscountDataCompundDTO> RelationDiscounts { get; set; } = Enumerable.Empty<DiscountDataCompundDTO>();
    }
}
