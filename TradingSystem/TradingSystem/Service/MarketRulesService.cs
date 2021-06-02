using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;

namespace TradingSystem.Service
{
    class MarketRulesService
    {
        private static readonly Lazy<MarketRulesService> instanceLazy = new Lazy<MarketRulesService>(() => new MarketRulesService(), true);

        private readonly MarketRules marketRules;
        private IDictionary<string, ICollection<IRule>> user_rules;
        int counter;

        private MarketRulesService()
        {
            marketRules = MarketRules.Instance;
            user_rules = new Dictionary<string, ICollection<IRule>>();
            counter = 0;
        }
        

        //Add New / Complex Discounts
        public async System.Threading.Tasks.Task<Guid> AddSimpleDiscountAsync(string username, Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "CreateSimpleDiscountAsync", username, storeId, discountType, precent, category, productId);
            return await marketRules.CreateSimpleDiscountAsync(username, storeId, discountType, precent, category, productId);
        }
        public async System.Threading.Tasks.Task<Guid> AddConditionalDiscountAsync(string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string ruleUsername = "",
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "CreateConditionalDiscountAsync", username, storeId, discountType, ruleType, precent, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
            return await marketRules.CreateConditionalDiscountAsync(username, storeId, discountType, ruleType, precent, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public async System.Threading.Tasks.Task<Guid> AddDiscountRuleAsync(string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId, Guid discountId2 = new Guid(), bool decide = false)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "GenerateConditionalDiscountsAsync", username, discountRuleRelation, storeId, ruleId1, ruleId2, discountId, discountId2, decide);
            return await marketRules.GenerateConditionalDiscountsAsync(username, discountRuleRelation, storeId, ruleId1, ruleId2, discountId, discountId2, decide);
        }

        public async System.Threading.Tasks.Task<Guid> RemoveDiscountAsync(string username, Guid storeId, Guid discountId)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "RemoveDiscountAsync", username, storeId, discountId);
            return await marketRules.RemoveDiscountAsync(username, storeId, discountId);
        }

        //Add New / Complex Policy
        public async Task AddPolicyRule(string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string ruleUsername = "",
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "AddPolicyRule", username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
            await marketRules.AddPolicyRule(username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
        }
        public async Task RemovePolicyRule(string username, Guid storeId)
        {
            await StorePredicatesManager.Instance.SaveRequest(counter++, "RemovePolicyRuleAsync", username, storeId);
            await marketRules.RemovePolicyRuleAsync(username, storeId);
        }

        private Guid CreateRule(string username, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string ruleUsername = "", double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            var r = marketRules.CreateRule(ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
            if (user_rules.Keys.Contains(username))
            {
                user_rules.Add(username, new HashSet<IRule>());
            }
            user_rules[username].Add(r);
            return r.GetId();
        }

    }
}
