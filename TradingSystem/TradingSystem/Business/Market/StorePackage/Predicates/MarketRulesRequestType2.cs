using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.PolicyPackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType2 : MarketRuleRequest
    {
        public int id { get; set; }
        public string functionName { get; set; }
        public string username { get; set; }
        public Guid storeId { get; set; }
        public PolicyRuleRelation policyRuleRelation { get; set; }
        public RuleContext ruleContext { get; set; }
        public RuleType ruleType { get; set; }
        public string category { get; set; }
        public Guid productId { get; set; }
        public double valueLessThan { get; set; }
        public double valueGreaterEQThan { get; set; }
        public DateTime d1 { get; set; }
        public DateTime d2 { get; set; }

        public MarketRulesRequestType2(int counter, string functionName, string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            this.id = counter;
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

        public MarketRulesRequestType2()
        {
        }

        public int getCounter()
        {
            return id;
        }

        public  void ActivateFunction(Store s)
        {
             MarketRules.Instance.AddPolicyRule(s,username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2).Wait();
            var policyData = new PolicyData(username, storeId, policyRuleRelation, ruleContext, ruleType, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
             MarketRulesService.Instance.policyManager.AddPolicy(policyData).Wait();
        }
    }
}