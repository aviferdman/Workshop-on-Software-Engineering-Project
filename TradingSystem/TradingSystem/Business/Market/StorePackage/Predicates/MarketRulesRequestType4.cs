using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType4
    {
        private int counter;
        private string functionName;
        private string username;
        private Guid storeId;
        private RuleContext discountType;
        private double precent;
        private string category;
        private Guid productId;

        public MarketRulesRequestType4(int counter, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountType = discountType;
            this.precent = precent;
            this.category = category;
            this.productId = productId;
        }
    }
}