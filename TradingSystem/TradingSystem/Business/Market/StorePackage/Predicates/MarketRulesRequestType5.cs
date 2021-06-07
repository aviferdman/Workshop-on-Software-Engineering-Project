using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType5: MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public DiscountRuleRelation discountRuleRelation { get; set; }
        public Guid storeId { get; set; }
        public Guid discountId { get; set; }
        public Guid discountId2 { get; set; }
        public bool decide { get; set; }

        public MarketRulesRequestType5(int counter, string functionName, string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId, Guid discountId2, bool decide)
        {
            this.id = counter;
            this.functionName = functionName;
            this.username = username;
            this.discountRuleRelation = discountRuleRelation;
            this.storeId = storeId;
            this.discountId = discountId;
            this.discountId2 = discountId2;
            this.decide = decide;
        }

        public MarketRulesRequestType5()
        {
        }
        public int getCounter()
        {
            return id;
        }

        public async Task ActivateFunction()
        {
            await MarketRules.Instance.GenerateConditionalDiscountsAsync(username, discountRuleRelation, storeId, discountId, discountId2, decide);
        }
    }
}