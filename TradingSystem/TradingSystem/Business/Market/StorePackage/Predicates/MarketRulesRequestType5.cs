using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

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
        public Guid originDiscountId { get; set; }

        public MarketRulesRequestType5(int counter, string functionName, string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId, Guid discountId2, bool decide, Guid originDiscountId)
        {
            this.id = counter;
            this.functionName = functionName;
            this.username = username;
            this.discountRuleRelation = discountRuleRelation;
            this.storeId = storeId;
            this.discountId = discountId;
            this.discountId2 = discountId2;
            this.decide = decide;
            this.originDiscountId = originDiscountId;
        }

        public MarketRulesRequestType5()
        {
        }
        public int getCounter()
        {
            return id;
        }

        public  void ActivateFunction(Store s)
        {
            var discountId1 = MarketRules.Instance.GenerateConditionalDiscountsAsync(s,username, discountRuleRelation, storeId, discountId, discountId2, decide).Result;
            var discountRelation = new DiscountsRelation(username, originDiscountId, discountRuleRelation, storeId, discountId, discountId2, decide);
            MarketRulesService.Instance.discountsManager.AddRelation(discountRelation).Wait();
        }
    }
}