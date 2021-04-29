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

        public Guid AddSimpleDiscount(Guid storeId, RuleContext discountType, double precent, string category = "", Guid productId = new Guid())
        {
            IStore store = marketStores.GetStoreById(storeId);
            var d = CreateCalculator(discountType, precent, category, productId);
            Discount discount = new Discount(d);
            return store.AddDiscount(discount);
        }
        public Guid AddConditionalDiscount(Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            IStore store = marketStores.GetStoreById(storeId);
            var d = CreateCalculator(discountType, precent, category, productId);
            var r = CreateRule(discountType, ruleType, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
            ConditionDiscount discount = new ConditionDiscount(d);
            discount.AddRule(r);
            return store.AddDiscount(discount);
        }

        public Guid AddRuleToStore(Guid storeId, RuleContext ruleContext, RuleType ruleType, double precent, string category = "", Guid productId = new Guid(), string username = "",
            double valueLessThan = double.MaxValue, double valueGreaterEQThan = 0, DateTime d1 = new DateTime(), DateTime d2 = new DateTime())
        {
            IStore store = marketStores.GetStoreById(storeId);
            var r = CreateRule(ruleContext, ruleType, category, productId, username, valueLessThan, valueGreaterEQThan, d1, d2);
            return store.AddRule(r);
        }

        public Guid AddPolicyAndRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var rule1 = store.GetRuleById(ruleId1);
            var rule2 = store.GetRuleById(ruleId2);
            var andRule = Rule.AddTwoRules(rule1, rule2);
            return store.AddRule(andRule);
        }

        public Guid AddPolicyOrRule(Guid storeId, Guid ruleId1, Guid ruleId2)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var rule1 = store.GetRuleById(ruleId1);
            var rule2 = store.GetRuleById(ruleId2);
            var orRule = Rule.OrTwoRules(rule1, rule2);
            return store.AddRule(orRule);
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

        public Guid AddXorDiscount(Guid storeId, Guid discountId1, Guid discountId2, bool decide)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var discount1 = (ConditionDiscount)store.GetDiscountById(discountId1);
            var discount2 = (ConditionDiscount)store.GetDiscountById(discountId2);
            var xorDiscount = discount1.Xor(discount2, decide);
            return store.AddDiscount(xorDiscount);
        }

        public Guid RemovePolicyRule(Guid storeId, Guid ruleId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            var rule = store.GetRuleById(ruleId);
            return store.RemoveRule(rule);
        }

        public Guid RemoveDiscount(Guid storeId, Guid discountId)
        {
            IStore store = marketStores.GetStoreById(storeId);
            return store.RemoveDiscount(discountId);
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
