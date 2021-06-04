using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType1: MarketRuleRequest
    {
        public int counter { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }
        public Guid discountId { get; set; } 

        public MarketRulesRequestType1(int counter, string functionName, string username, Guid storeId, Guid discountId)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountId = discountId;
        }

        public MarketRulesRequestType1()
        {
        }

        public int getCounter()
        {
            return counter;
        }

        public async Task ActivateFunction()
        {
            await MarketRules.Instance.RemoveDiscountAsync(username, storeId, discountId);      
        }
    }
}