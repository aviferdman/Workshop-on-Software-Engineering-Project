using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType8: MarketRuleRequest
    {
        private int counter;
        private int id;
        private Guid existingDiscountId;
        private string functionName;
        private string username;
        private Guid storeId;
        private RuleContext discountType;
        private RuleType ruleType;
        private double precent;
        private string category;
        private Guid productId;
        private double valueLessThan;
        private double valueGreaterEQThan;
        private DateTime d1;
        private DateTime d2;
        private Guid originDiscountId;

        public MarketRulesRequestType8()
        {
        }

        public MarketRulesRequestType8(int counter, Guid existingDiscountId, string functionName, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId, double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2, Guid originDiscountId)
        {
            this.id = counter;
            this.ExistingDiscountId = existingDiscountId;
            this.FunctionName = functionName;
            this.Username = username;
            this.StoreId = storeId;
            this.DiscountType = discountType;
            this.RuleType = ruleType;
            this.Precent = precent;
            this.Category = category;
            this.ProductId = productId;
            this.ValueLessThan = valueLessThan;
            this.ValueGreaterEQThan = valueGreaterEQThan;
            this.D1 = d1;
            this.D2 = d2;
            this.OriginDiscountId = originDiscountId;
        }

        public int Id { get => id; set => id = value; }
        public Guid ExistingDiscountId { get => existingDiscountId; set => existingDiscountId = value; }
        public string FunctionName { get => functionName; set => functionName = value; }
        public string Username { get => username; set => username = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public RuleContext DiscountType { get => discountType; set => discountType = value; }
        public RuleType RuleType { get => ruleType; set => ruleType = value; }
        public double Precent { get => precent; set => precent = value; }
        public string Category { get => category; set => category = value; }
        public Guid ProductId { get => productId; set => productId = value; }
        public double ValueLessThan { get => valueLessThan; set => valueLessThan = value; }
        public double ValueGreaterEQThan { get => valueGreaterEQThan; set => valueGreaterEQThan = value; }
        public DateTime D1 { get => d1; set => d1 = value; }
        public DateTime D2 { get => d2; set => d2 = value; }
        public Guid OriginDiscountId { get => originDiscountId; set => originDiscountId = value; }

        public  void ActivateFunction(Store s)
        {
            var res =  MarketRules.Instance.UpdateConditionalDiscountAsync(s, existingDiscountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2, originalDiscountId: OriginDiscountId).Result;
            Guid discountId = res.Ret;
            var discountData = new DiscountData(OriginDiscountId, username, storeId, discountType, ruleType, precent, category, productId, valueLessThan, valueGreaterEQThan, d1, d2);
             MarketRulesService.Instance.discountsManager.RemoveDiscount(discountId).Wait();
             MarketRulesService.Instance.discountsManager.AddDiscount(discountData).Wait();
        }

        public int getCounter()
        {
           return id;
        }
    }
}