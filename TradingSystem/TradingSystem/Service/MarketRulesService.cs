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

        public Guid AddDiscountRule(DiscountRuleRelation discountRuleRelation, Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId, Guid discountId2 = new Guid(), bool decide = false)
        {
            return marketRules.AddDiscountRule(discountRuleRelation, storeId, ruleId1, ruleId2, discountId, discountId2, decide);
        }

        public Guid RemoveDiscount(Guid storeId, Guid discountId)
        {
            return marketRules.RemoveDiscount(storeId, discountId);
        }

        public void RemovePolicyRule(Guid storeId)
        {
            marketRules.RemovePolicyRule(storeId);
        }

        public void AddPolicyRule(Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            marketRules.AddPolicyRule(storeId, policyRuleRelation, ruleContext, ruleType, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
        }
    }
}
