using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType3
    {
        private int counter;
        private string functionName;
        private string username;
        private Guid storeId;

        public MarketRulesRequestType3(int counter, string functionName, string username, Guid storeId)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
        }
    }
}