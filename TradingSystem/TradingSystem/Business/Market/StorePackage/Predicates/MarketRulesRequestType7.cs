using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage.Predicates;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType7: MarketRuleRequest
    {
        private int id;
        private Guid existingDiscountId;
        private string functionName;
        private string username;
        private Guid storeId;
        private RuleContext discountType;
        private double precent;
        private string category;
        private Guid productId;

        public MarketRulesRequestType7()
        {
        }

        public MarketRulesRequestType7(int counter, Guid existingDiscountId, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId)
        {
            this.Id = counter;
            this.ExistingDiscountId = existingDiscountId;
            this.FunctionName = functionName;
            this.Username = username;
            this.StoreId = storeId;
            this.DiscountType = discountType;
            this.Precent = precent;
            this.Category = category;
            this.ProductId = productId;
        }

        public int Id { get => id; set => id = value; }
        public Guid ExistingDiscountId { get => existingDiscountId; set => existingDiscountId = value; }
        public string FunctionName { get => functionName; set => functionName = value; }
        public string Username { get => username; set => username = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public RuleContext DiscountType { get => discountType; set => discountType = value; }
        public double Precent { get => precent; set => precent = value; }
        public string Category { get => category; set => category = value; }
        public Guid ProductId { get => productId; set => productId = value; }

        public async Task ActivateFunction()
        {
            await MarketRules.Instance.UpdateSimpleDiscountAsync(existingDiscountId, username, storeId, discountType, precent, category, productId);
        }

        public int getCounter()
        {
            return id;
        }
    }
}