using System;

namespace TradingSystem.Business.Market.StorePackage
{
    internal class MarketRulesRequestType5
    {
        private int counter;
        private string functionName;
        private string username;
        private DiscountRuleRelation discountRuleRelation;
        private Guid storeId;
        private Guid discountId;
        private Guid discountId2;
        private bool decide;

        public MarketRulesRequestType5(int counter, string functionName, string username, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId, Guid discountId2, bool decide)
        {
            this.counter = counter;
            this.functionName = functionName;
            this.username = username;
            this.discountRuleRelation = discountRuleRelation;
            this.storeId = storeId;
            this.discountId = discountId;
            this.discountId2 = discountId2;
            this.decide = decide;
        }
    }
}