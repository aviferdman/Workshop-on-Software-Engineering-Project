using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    class MarketRulesService
    {
        private static readonly Lazy<MarketRulesService> instanceLazy = new Lazy<MarketRulesService>(() => new MarketRulesService(), true);

        private readonly MarketRules marketRules;

        private MarketRulesService()
        {
            marketRules = MarketRules.Instance;
        }

        public void AddSimpleDiscount(Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            marketRules.AddSimpleDiscount(storeId, discountType, precent, category, productId);
        }
        public void AddConditionalDiscount(Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            marketRules.AddConditionalDiscount(storeId, discountType, ruleType, precent, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public void AddRuleToStore(Guid storeId, RuleContext ruleContext, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            marketRules.AddRuleToStore(storeId, ruleContext, ruleType, precent, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public void AddPolicyAndRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            marketRules.AddPolicyAndRule(storeId, ruleId1, ruleId2);
        }

        public void AddPolicyOrRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            marketRules.AddPolicyOrRule(storeId, ruleId1, ruleId2);
        }

        public void AddDiscountAndRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            marketRules.AddDiscountAndRule(storeId, ruleId1, ruleId2, discountId);
        }

        public void AddDiscountOrRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            marketRules.AddDiscountOrRule(storeId, ruleId1, ruleId2, discountId);
        }

        public void AddXorDiscount(Guid storeId, Guid discountId1, Guid discountId2, bool decide)
        {
            marketRules.AddXorDiscount(storeId, discountId1, discountId2, decide);
        }

        public void RemovePolicyRule(Guid storeId, Guid ruleId)
        {
            marketRules.RemovePolicyRule(storeId, ruleId);
        }

        public void RemoveDiscount(Guid storeId, Guid discountId)
        {
            marketRules.RemoveDiscount(storeId, discountId);
        }
    }
}
