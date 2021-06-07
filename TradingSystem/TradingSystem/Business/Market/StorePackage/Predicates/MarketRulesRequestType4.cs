using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType4: MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }
        public RuleContext discountType { get; set; }
        public double precent { get; set; }
        public string category { get; set; }
        public Guid productId { get; set; }

        public MarketRulesRequestType4(int counter, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId)
        {
            this.id = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountType = discountType;
            this.precent = precent;
            this.category = category;
            this.productId = productId;
        }

        public MarketRulesRequestType4()
        {
        }

        public int getCounter()
        {
            return id;
        }

        public async Task ActivateFunction()
        {
            await MarketRules.Instance.CreateSimpleDiscountAsync(username, storeId, discountType, precent, category, productId);
        }
    }
}