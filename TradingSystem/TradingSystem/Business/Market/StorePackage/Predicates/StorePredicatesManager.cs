using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market.StorePackage
{
    class StorePredicatesManager
    {
        private Dictionary<Guid, List<Discount>> _store_discounts;
        private Dictionary<Guid, Policy> _store_policy;
        private MarketDAL marketDAL = MarketDAL.Instance;
        private static readonly Lazy<StorePredicatesManager>
        _lazy =
        new Lazy<StorePredicatesManager>
            (() => new StorePredicatesManager());

        public static StorePredicatesManager Instance { get { return _lazy.Value; } }

        private StorePredicatesManager()
        {
            this._store_discounts = new Dictionary<Guid, List<Discount>>();
            this._store_policy = new Dictionary<Guid, Policy>();
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
            await marketDAL.AddRequestType1(marketRulesRequest);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            var marketRulesRequest = new MarketRulesRequestType2(counter, functionName, username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            await marketDAL.AddRequestType2(marketRulesRequest);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId)
        {
            var marketRulesRequest = new MarketRulesRequestType3(counter, functionName, username, storeId);
            await marketDAL.AddRequestType3(marketRulesRequest);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId, Guid originDiscountId)
        {
            var marketRulesRequest = new MarketRulesRequestType4(counter, functionName, username, storeId, discountType, precent, category, productId, originDiscountId);
            await marketDAL.AddRequestType4(marketRulesRequest);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId, Guid discountId2, bool decide, Guid originDiscountId)
        {
            var marketRulesRequest = new MarketRulesRequestType5(counter, functionName, username, discountRuleRelation, storeId, discountId, discountId2, decide, originDiscountId);
            await marketDAL.AddRequestType5(marketRulesRequest);
        }

        internal async Task SaveRequest(int counter, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2, Guid originDiscountId)
        {
            var marketRulesRequest = new MarketRulesRequestType6(counter, functionName, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2, originDiscountId);
            await marketDAL.AddRequestType6(marketRulesRequest);
        }


        public async Task SaveRequest(int counter, Guid existingDiscountId, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId, Guid originDiscountId)
        {
            var marketRulesRequest = new MarketRulesRequestType7(counter, existingDiscountId, functionName, username, storeId, discountType, precent, category, productId, originDiscountId);
             await marketDAL.AddRequestType7(marketRulesRequest);
        }

        public async Task SaveRequest(int counter, Guid existingDiscountId, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2, Guid originDiscountId)
        {
            var marketRulesRequest = new MarketRulesRequestType8(counter, existingDiscountId, functionName, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2, originDiscountId);
             await marketDAL.AddRequestType8(marketRulesRequest);
        }

    }
}
