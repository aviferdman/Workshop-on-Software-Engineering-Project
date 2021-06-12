using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType6: MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }
        public RuleContext discountType { get; set; }
        public RuleType ruleType { get; set; }
        public double precent { get; set; }
        public string category { get; set; }
        public Guid productId { get; set; }
        public double valueLessThan { get; set; }
        public double valueGreaterEQThan { get; set; }
        public DateTime d1 { get; set; }
        public DateTime d2 { get; set; }
        public Guid originDiscountId { get; set; }


        public MarketRulesRequestType6(int counter, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2, Guid originDiscountId)
        {
            this.id = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountType = discountType;
            this.ruleType = ruleType;
            this.precent = precent;
            this.category = category;
            this.productId = productId;
            this.valueLessThan = valueLessThan;
            this.valueGreaterEQThan = valueGreaterEQThan;
            this.d1 = d1;
            this.d2 = d2;
            this.originDiscountId = originDiscountId;
        }

        public MarketRulesRequestType6()
        {
        }
        public int getCounter()
        {
            return id;
        }

        public  void ActivateFunction(Store s)
        {
            var discountId = MarketRules.Instance.CreateConditionalDiscountAsync(s,username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2, originalDiscountId: originDiscountId).Result;
            var discountData = new DiscountData(originDiscountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
            MarketRulesService.Instance.discountsManager.AddDiscount(discountData).Wait();
        }
    }
}