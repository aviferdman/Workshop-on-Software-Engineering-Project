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

        public Guid AddSimpleDiscount(Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            return marketRules.AddSimpleDiscount(storeId, discountType, precent, category, productId);
        }
        public Guid AddConditionalDiscount(Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            return marketRules.AddConditionalDiscount(storeId, discountType, ruleType, precent, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public Guid AddRuleToStore(Guid storeId, RuleContext ruleContext, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            return marketRules.AddRuleToStore(storeId, ruleContext, ruleType, precent, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public Guid AddPolicyAndRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            return marketRules.AddPolicyAndRule(storeId, ruleId1, ruleId2);
        }

        public Guid AddPolicyOrRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            return marketRules.AddPolicyOrRule(storeId, ruleId1, ruleId2);
        }

        public Guid AddDiscountAndRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            return marketRules.AddDiscountAndRule(storeId, ruleId1, ruleId2, discountId);
        }

        public Guid AddDiscountOrRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            return marketRules.AddDiscountOrRule(storeId, ruleId1, ruleId2, discountId);
        }

        public Guid AddXorDiscount(Guid storeId, Guid discountId1, Guid discountId2, bool decide)
        {
            return marketRules.AddXorDiscount(storeId, discountId1, discountId2, decide);
        }

        public Guid RemovePolicyRule(Guid storeId, Guid ruleId)
        {
            return marketRules.RemovePolicyRule(storeId, ruleId);
        }

        public Guid RemoveDiscount(Guid storeId, Guid discountId)
        {
            return marketRules.RemoveDiscount(storeId, discountId);
        }
    }
}
