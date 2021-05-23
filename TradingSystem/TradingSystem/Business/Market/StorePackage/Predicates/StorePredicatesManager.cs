﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market.StorePackage
{
    class StorePredicatesManager
    {
        private Dictionary<Guid, List<Discount>> _store_discounts;
        private Dictionary<Guid, Policy> _store_policy;

        private static readonly Lazy<StorePredicatesManager>
        _lazy =
        new Lazy<StorePredicatesManager>
            (() => new StorePredicatesManager());

        public static StorePredicatesManager Instance { get { return _lazy.Value; } }

        private StorePredicatesManager()
        {
            this._store_discounts = new Dictionary<Guid, List<Discount>>();
            this._store_policy = new Dictionary<Guid, Policy>();
            RestorePredicates();
        }

        public void AddDiscount(Guid storeId, Discount discount)
        {
            if (!_store_discounts.ContainsKey(storeId))
            {
                _store_discounts.Add(storeId, new List<Discount>());
            }
            _store_discounts[storeId].Add(discount);
        }

        public List<Discount> GetDiscounts(Guid storeId)
        {
            if (!_store_discounts.ContainsKey(storeId))
            {
                _store_discounts.Add(storeId, new List<Discount>());
            }
            return _store_discounts[storeId];
        }

        public Discount GetDiscountById(Guid storeId, Guid discountId)
        {
            if (!_store_discounts.ContainsKey(storeId))
            {
                _store_discounts.Add(storeId, new List<Discount>());
            }
            return _store_discounts[storeId].Where(d => d.Id.Equals(discountId)).FirstOrDefault();
        }

        public Policy GetPolicy(Guid storeId)
        {
            if (!_store_policy.ContainsKey(storeId))
            {
                _store_policy.Add(storeId, new Policy());
            }
            return _store_policy[storeId];
        }

        public void SetPolicy(Guid storeId, Policy policy)
        {
            if (!_store_policy.ContainsKey(storeId))
            {
                _store_policy.Add(storeId, policy);
            }
            else
            {
                _store_policy[storeId] = policy;
            }
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, Guid discountId)
        {
            var marketRulesRequest = new MarketRulesRequestType1(counter, functionName, username, storeId, discountId);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category, Guid productId, string ruleUsername, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            var marketRulesRequest = new MarketRulesRequestType2(counter, functionName, username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);

        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId)
        {
            var marketRulesRequest = new MarketRulesRequestType3(counter, functionName, username, storeId);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId)
        {
            var marketRulesRequest = new MarketRulesRequestType4(counter, functionName, username, storeId, discountType, precent, category, productId);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId, Guid discountId2, bool decide)
        {
            var marketRulesRequest = new MarketRulesRequestType5(counter, functionName, username, discountRuleRelation, storeId, ruleId1, ruleId2, discountId, discountId2, decide);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, string ruleUsername, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            var marketRulesRequest = new MarketRulesRequestType6(counter, functionName, username, storeId, discountType, ruleType, precent, category, productId, ruleUsername, valueLessThan, valueGreaterEQThan, d1, d2);
        }
        private void RestorePredicates()
        {
            // restore from db the requests
            // activate functions by the 'counter' order
            // activate the matching function by the 'functionName' from the class MarketRules

            
        }
    }
}