using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.PolicyPackage;

namespace TradingSystem.WebApi.DTO.Store.Policy
{
    public class PolicyDataDTO
    {
        public Guid Id { get; set; }
        public string Creator { get; set; } = "";

        public string RuleRelation { get; set; } = "";
        public string RuleContext { get; set; } = "";
        public string? RuleType { get; set; } = "";

        public string? Category { get; set; }
        public Guid ProductId { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public static PolicyDataDTO FromPolicyData(PolicyData policyData)
        {
            return new PolicyDataDTO
            {
                // just here to serve as a key for the client
                Id = Guid.NewGuid(),
                Creator = policyData.Username,

                RuleRelation = policyData.PolicyRuleRelation.ToString(),
                RuleContext = policyData.RuleContext.ToString(),
                RuleType = policyData.RuleType == Business.Market.RuleType.Simple ? null : policyData.RuleType.ToString(),

                Category = policyData.Category,
                ProductId = policyData.ProductId,
                MinValue = policyData.ValueGreaterEQThan == 0 ? null : (int?)policyData.ValueGreaterEQThan,
                MaxValue = policyData.ValueLessThan == int.MaxValue ? null : (int?)policyData.ValueLessThan,
                StartDate = policyData.D1 == default ? null : (DateTime?)policyData.D1,
                EndDate = policyData.D2 == default ? null : (DateTime?)policyData.D2,
            };
        }
    }
}
