using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType2
    {
        private int counter;
        private string functionName;
        private string username;
        private Guid storeId;
        private PolicyRuleRelation policyRuleRelation;
        private RuleContext ruleContext;
        private RuleType ruleType;
        private string category;
        private Guid productId;
        private double valueLessThan;
        private double valueGreaterEQThan;
        private DateTime d1;
        private DateTime d2;

        public MarketRulesRequestType2(int counter, string functionName, string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.storeId = storeId;
            this.policyRuleRelation = policyRuleRelation;
            this.ruleContext = ruleContext;
            this.ruleType = ruleType;
            this.category = category;
            this.productId = productId;
            this.valueLessThan = valueLessThan;
            this.valueGreaterEQThan = valueGreaterEQThan;
            this.d1 = d1;
            this.d2 = d2;
        }
    }
}