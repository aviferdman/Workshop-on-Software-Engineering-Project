using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType1
    {
        private int counter;
        private string functionName;
        private string username;
        private Guid storeId;
        private Guid discountId;

        public MarketRulesRequestType1(int counter, string functionName, string username, Guid storeId, Guid discountId)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountId = discountId;
        }
    }
}