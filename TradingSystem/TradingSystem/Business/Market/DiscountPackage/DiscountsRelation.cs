using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.DiscountPackage
{
    public class DiscountsRelation
    {
        private int serialNumber;
        private Guid id;
        private string _username;
        private DiscountRuleRelation _discountRuleRelation;
        private Guid _storeId;
        private Guid _discountId1;
        private Guid _discountId2;
        private bool _decide;
        public DiscountsRelation(string username, Guid discountId, DiscountRuleRelation discountRuleRelation, Guid storeId, Guid discountId1, Guid discountId2, bool decide)
        {
            this.Id = discountId;
            this.Username = username;
            this.DiscountRuleRelation = discountRuleRelation;
            this.StoreId = storeId;
            this.DiscountId1 = discountId1;
            this.DiscountId2 = discountId2;
            this.Decide = decide;
        }

        public string Username { get => _username; set => _username = value; }
        public DiscountRuleRelation DiscountRuleRelation { get => _discountRuleRelation; set => _discountRuleRelation = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public Guid DiscountId1 { get => _discountId1; set => _discountId1 = value; }
        public Guid DiscountId2 { get => _discountId2; set => _discountId2 = value; }
        public bool Decide { get => _decide; set => _decide = value; }
        public Guid Id { get => id; set => id = value; }
        public int SerialNumber { get => serialNumber; set => serialNumber = value; }
    }
}
