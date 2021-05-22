using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType6
    {
        private int counter;
        private string functionName;
        private string username;
        private Guid storeId;
        private RuleContext discountType;
        private RuleType ruleType;
        private double precent;
        private string category;
        private Guid productId;
        private string ruleUsername;
        private double valueLessThan;
        private double valueGreaterEQThan;
        private DateTime d1;
        private DateTime d2;

        public MarketRulesRequestType6(int counter, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, string ruleUsername, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.discountType = discountType;
            this.ruleType = ruleType;
            this.precent = precent;
            this.category = category;
            this.productId = productId;
            this.ruleUsername = ruleUsername;
            this.valueLessThan = valueLessThan;
            this.valueGreaterEQThan = valueGreaterEQThan;
            this.d1 = d1;
            this.d2 = d2;
        }
    }
}