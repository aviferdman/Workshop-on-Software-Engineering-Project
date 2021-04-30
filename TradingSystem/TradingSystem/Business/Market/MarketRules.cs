using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.RulesPackage;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;

namespace TradingSystem.Business.Market
{
    public enum RuleContext
    {
        Store,
        Category,
        Product
    }
    public enum RuleType
    {
        Age,
        Quantity,
        Price,
        Weight,
        Time
    }
    public enum PolicyRuleRelation
    {
        Simple,
        And,
        Or,
        Condition
    }
    public enum DiscountRuleRelation
    {
        And,
        Or,
        Xor
    }
    public class MarketRules
    {
        private RulesCreator rulesCreator;
        private IMarketStores marketStores;

        private static readonly Lazy<MarketRules>
        _lazy =
        new Lazy<MarketRules>
            (() => new MarketRules());

        public static MarketRules Instance { get { return _lazy.Value; } }

        private MarketRules()
        {
            this.rulesCreator = new RulesCreator();
            this.marketStores = MarketStores.Instance;
        }

        public Guid CreateSimpleDiscount(Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            IStore store = marketStores.GetStoreById(storeId);
            var d = CreateCalculator(discountType, precent, category, productId);
            Discount discount = new Discount(d);
            return store.AddDiscount(discount);
        }
        public Guid CreateConditionalDiscount(Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            IStore store = marketStores.GetStoreById(storeId);
            var d = CreateCalculator(discountType, precent, category, productId);
            var r = CreateRule(discountType, ruleType, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
            ConditionDiscount discount = new ConditionDiscount(d);
            discount.AddRule(r);
            return store.AddDiscount(discount);
        }

        public Guid GenerateConditionalDiscounts(DiscountRuleRelation discountRuleRelation, Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId, Guid discountId2, bool decide)
        {
            switch (discountRuleRelation)
            {
                case DiscountRuleRelation.And:
                    return AddDiscountAndRule(storeId, ruleId1, ruleId2, discountId);
                case DiscountRuleRelation.Or:
                    return AddDiscountOrRule(storeId, ruleId1, ruleId2, discountId);
                default:
                    return AddDiscountXorRule(storeId, discountId, discountId2, decide);
            }
        }

        public Guid AddDiscountAndRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var rule1 = store.GetRuleById(ruleId1);
            var rule2 = store.GetRuleById(ruleId2);
            var andRule = Rule.AddTwoRules(rule1, rule2);
            var discount = (ConditionDiscount)store.GetDiscountById(discountId);
            discount.AddRule(andRule);
            return store.AddDiscount(discount);
        }

        public Guid AddDiscountOrRule(Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var rule1 = store.GetRuleById(ruleId1);
            var rule2 = store.GetRuleById(ruleId2);
            var andRule = Rule.OrTwoRules(rule1, rule2);
            var discount = (ConditionDiscount)store.GetDiscountById(discountId);
            discount.AddRule(andRule);
            return store.AddDiscount(discount);
        }

        public Guid AddDiscountXorRule(Guid storeId, Guid discountId1, Guid discountId2, bool decide)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var discount1 = (ConditionDiscount)store.GetDiscountById(discountId1);
            var discount2 = (ConditionDiscount)store.GetDiscountById(discountId2);
            var xorDiscount = discount1.Xor(discount2, decide);
            return store.AddDiscount(xorDiscount);
        }

        public Guid RemoveDiscount(Guid storeId, Guid discountId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            return store.RemoveDiscount(discountId);
        }

        public void AddPolicyRule(Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            var r = CreateRule(ruleContext, ruleType, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
            switch (policyRuleRelation)
            {
                case PolicyRuleRelation.Simple:
                    CreateRuleToStore(storeId, r);
                    break;
                case PolicyRuleRelation.And:
                    GeneratePolicyAndRule(storeId, r);
                    break;
                case PolicyRuleRelation.Or:
                    GeneratePolicyOrRule(storeId, r);
                    break;
                default:
                    GeneratePolicyConditionRule(storeId, r);
                    break;
            }
        }

        public Guid CreateRuleToStore(Guid storeId, Rule rule)
        {
            IStore store = marketStores.GetStoreById(storeId);
            return store.AddRule(rule);
        }

        public void GeneratePolicyAndRule(Guid storeId, Rule rule)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var newPolicy = store.GetPolicy().And(rule);
            store.SetPolicy(newPolicy);
        }

        public void GeneratePolicyOrRule(Guid storeId, Rule rule)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var newPolicy = store.GetPolicy().Or(rule);
            store.SetPolicy(newPolicy);
        }

        public void GeneratePolicyConditionRule(Guid storeId, Rule rule)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var newPolicy = store.GetPolicy().Condition(rule);
            store.SetPolicy(newPolicy);
        }

        public void RemovePolicyRule(Guid storeId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            store.RemoveRule();
        }

        private Rule CreateRule(RuleContext discountType, RuleType ruleType, string category, Guid productId, string username, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            Rule r;
            switch (ruleType)
            {
                case RuleType.Age:
                    r = rulesCreator.CreateUserAgeRule(username, Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                    break;
                case RuleType.Price:
                    r = rulesCreator.CreateStorePriceRule(Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                    break;
                case RuleType.Quantity:
                    switch (discountType)
                    {
                        case RuleContext.Store:
                            r = rulesCreator.CreateStoreRule(Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                            break;
                        case RuleContext.Category:
                            r = rulesCreator.CreateCategoryRule(category, Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                            break;
                        default:
                            r = rulesCreator.CreateProductRule(productId, Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                            break;
                    }
                    break;
                case RuleType.Weight:
                    r = rulesCreator.CreateProductWeightRule(productId, Convert.ToInt32(valueLessThan), Convert.ToInt32(valueGreaterEQThan));
                    break;
                default:
                    r = rulesCreator.CreateTimeRule(d1, d2);
                    break;
            }
            return r;
        }

        private IDiscountCalculator CreateCalculator(RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            IDiscountCalculator d;
            switch (discountType)
            {
                case RuleContext.Store:
                    d = new StoreDiscountCalculator(precent);
                    break;
                case RuleContext.Category:
                    d = new CategoryDiscountCalculator(category, precent);
                    break;
                default:
                    d = new ProductDiscountCalculator(productId, precent);
                    break;
            }
            return d;
        }
    }
}
