using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType1: MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }
        public Guid discountId { get; set; } 

        public MarketRulesRequestType1(int counter, string functionName, string username, Guid storeId, Guid discountId)
        {
            this.id = counter;
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
            return id;
        }

        public async Task ActivateFunction(Store s)
        {
            await MarketRules.Instance.RemoveDiscountAsync(s,username, storeId, discountId);
            await MarketRulesService.Instance.discountsManager.RemoveDiscount(discountId);
        }
    }
}