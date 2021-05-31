using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.StorePackage.PolicyPackage
{
    public class PolicyData
    {
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

        private int time;

        public PolicyData(string username, Guid storeId, PolicyRuleRelation policyRuleRelation, RuleContext ruleContext, RuleType ruleType, string category,
                    Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            this.Username = username;
            this.StoreId = storeId;
            this.PolicyRuleRelation = policyRuleRelation;
            this.RuleContext = ruleContext;
            this.RuleType = ruleType;
            this.Category = category;
            this.ProductId = productId;
            this.ValueLessThan = valueLessThan;
            this.ValueGreaterEQThan = valueGreaterEQThan;
            this.D1 = d1;
            this.D2 = d2;
        }
        public string Username { get => username; set => username = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public PolicyRuleRelation PolicyRuleRelation { get => policyRuleRelation; set => policyRuleRelation = value; }
        public RuleContext RuleContext { get => ruleContext; set => ruleContext = value; }
        public RuleType RuleType { get => ruleType; set => ruleType = value; }
        public string Category { get => category; set => category = value; }
        public Guid ProductId { get => productId; set => productId = value; }
        public double ValueLessThan { get => valueLessThan; set => valueLessThan = value; }
        public double ValueGreaterEQThan { get => valueGreaterEQThan; set => valueGreaterEQThan = value; }
        public DateTime D1 { get => d1; set => d1 = value; }
        public DateTime D2 { get => d2; set => d2 = value; }
        public int Time { get => time; set => time = value; }
    }
}
