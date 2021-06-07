using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType3: MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }

        public MarketRulesRequestType3(int counter, string functionName, string username, Guid storeId)
        {
            this.id = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
        }

        public MarketRulesRequestType3()
        {
        }
        public int getCounter()
        {
            return id;
        }

        public async Task ActivateFunction()
        {
            await MarketRules.Instance.RemovePolicyRuleAsync(username, storeId);
        }
    }
}