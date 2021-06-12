using System;
using System.Threading.Tasks;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.Predicates;
using TradingSystem.Service;

namespace TradingSystem.Business.Market.StorePackage
{
    public class MarketRulesRequestType7: MarketRuleRequest
    {
        private int counter;
        private Guid existingDiscountId;
        private string functionName;
        private string username;
        private Guid storeId;
        private RuleContext discountType;
        private double precent;
        private string category;
        private Guid productId;
        private Guid originDiscountId;

        public MarketRulesRequestType7()
        {
        }

        public MarketRulesRequestType7(int counter, Guid existingDiscountId, string functionName, string username, Guid storeId, RuleContext discountType, double precent, string category, Guid productId, Guid originDiscountId)
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
            this.OriginDiscountId = originDiscountId;
        }

        public int Id { get => counter; set => counter = value; }
        public Guid ExistingDiscountId { get => existingDiscountId; set => existingDiscountId = value; }
        public string FunctionName { get => functionName; set => functionName = value; }
        public string Username { get => username; set => username = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public RuleContext DiscountType { get => discountType; set => discountType = value; }
        public double Precent { get => precent; set => precent = value; }
        public string Category { get => category; set => category = value; }
        public Guid ProductId { get => productId; set => productId = value; }
        public Guid OriginDiscountId { get => originDiscountId; set => originDiscountId = value; }

        public  void ActivateFunction(Store s)
        {
            var res =  MarketRules.Instance.UpdateSimpleDiscountAsync(s,existingDiscountId, username, storeId, discountType, precent, category, productId, originalDiscountId: OriginDiscountId).Result;
            Guid discountId = res.Ret;
            var discountData = new DiscountData(OriginDiscountId, username, storeId, discountType, RuleType.Simple, precent, category, productId, int.MaxValue, 0, default(DateTime), default(DateTime));
            MarketRulesService.Instance.discountsManager.RemoveDiscount(discountId).Wait();
            MarketRulesService.Instance.discountsManager.AddDiscount(discountData).Wait();
        }

        public int getCounter()
        {
            return Id;
        }
    }
}