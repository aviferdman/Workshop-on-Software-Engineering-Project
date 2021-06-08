using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;

namespace TradingSystem.WebApi.DTO.Store.Discounts
{
    public class DiscountDataDTO
    {
        public Guid Id { get; set; }
        public string Creator { get; set; } = "";
        public string DiscountType { get; set; } = "";
        public string? ConditionType { get; set; }
        public double Percent { get; set; }
        public string? Category { get; set; }
        public Guid ProductId { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public static DiscountDataDTO FromDiscountData(DiscountData discountData)
        {
            return new DiscountDataDTO
            {
                Id = discountData.DiscountId,
                Creator = discountData.Username,
                DiscountType = discountData.DiscountType.ToString(),
                ConditionType = discountData.RuleType == RuleType.Simple ? null : discountData.RuleType.ToString(),
                Percent = discountData.Precent,
                Category = discountData.Category,
                ProductId = discountData.ProductId,
                MinValue = discountData.ValueGreaterEQThan == 0 ? null : (int?)discountData.ValueGreaterEQThan,
                MaxValue = discountData.ValueLessThan == int.MaxValue ? null : (int?)discountData.ValueLessThan,
                StartDate = discountData.D1 == default ? null : (DateTime?)discountData.D1,
                EndDate = discountData.D2 == default ? null : (DateTime?)discountData.D2,
            };
        }
    }
}
