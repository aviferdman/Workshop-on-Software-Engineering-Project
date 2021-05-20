﻿using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    class MarketRulesService
    {
        private static readonly Lazy<MarketRulesService> instanceLazy = new Lazy<MarketRulesService>(() => new MarketRulesService(), true);

        private readonly MarketRules marketRules;
        private IDictionary<string, ICollection<IRule>> user_rules;

        private MarketRulesService()
        {
            marketRules = MarketRules.Instance;
            user_rules = new Dictionary<string, ICollection<IRule>>();
        }

        public Guid CreateRule(string username, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string ruleUsername = "", double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            var r = marketRules.CreateRule(ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
            if (user_rules.Keys.Contains(username))
            {
                user_rules.Add(username, new HashSet<IRule>());
            }
            user_rules[username].Add(r);
            return r.GetId();
        }


        //Add New / Complex Discounts
        public Guid AddSimpleDiscount(string username, Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            return marketRules.CreateSimpleDiscountAsync(username, storeId, discountType, precent, category, productId);
        }
        public Guid AddConditionalDiscount(string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string ruleUsername = "",
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            return marketRules.CreateConditionalDiscountAsync(username, storeId, discountType, ruleType, precent, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
        }

        public Guid AddDiscountRule(string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId, Guid discountId2 = new Guid(), bool decide = false)
        {
            return marketRules.GenerateConditionalDiscountsAsync(username, discountRuleRelation, storeId, ruleId1, ruleId2, discountId, discountId2, decide);
        }

        public Guid RemoveDiscount(string username, Guid storeId, Guid discountId)
        {
            return marketRules.RemoveDiscountAsync(username, storeId, discountId);
        }

        //Add New / Complex Policy
        public void AddPolicyRule(string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string ruleUsername = "",
                                        double valueLessThan = int.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            marketRules.AddPolicyRule(username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
        }
        public void RemovePolicyRule(string username, Guid storeId)
        {
            marketRules.RemovePolicyRuleAsync(username, storeId);
        }

    }
}
