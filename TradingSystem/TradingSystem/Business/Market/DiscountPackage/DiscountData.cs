using System;

namespace TradingSystem.Business.Market.DiscountPackage
{
    public class DiscountData
    {
        private Guid _discountId;
        private string _username;
        private Guid _storeId;
        private RuleContext _discountType;
        private RuleType _ruleType;
        private double _precent;
        private string _category;
        private Guid _productId;
        private double _valueLessThan;
        private double _valueGreaterEQThan;
        private DateTime _d1;
        private DateTime _d2;
        public DiscountData(Guid discountId, string username, Guid storeId, RuleContext discountType, RuleType ruleType, double precent, string category, Guid productId,
                                        double valueLessThan, double valueGreaterEQThan, DateTime d1, DateTime d2)
        {
            this._discountId = discountId;
            this._username = username;
            this._storeId = storeId;
            this._discountType = discountType;
            this._ruleType = ruleType;
            this._precent = precent;
            this._category = category;
            this._productId = productId;
            this._valueLessThan = valueLessThan;
            this._valueGreaterEQThan = valueGreaterEQThan;
            this._d1 = d1;
            this._d2 = d2;
        }

        public Guid DiscountId { get => _discountId; set => _discountId = value; }
        public string Username { get => _username; set => _username = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public RuleContext DiscountType { get => _discountType; set => _discountType = value; }
        public RuleType RuleType { get => _ruleType; set => _ruleType = value; }
        public double Precent { get => _precent; set => _precent = value; }
        public string Category { get => _category; set => _category = value; }
        public Guid ProductId { get => _productId; set => _productId = value; }
        public double ValueLessThan { get => _valueLessThan; set => _valueLessThan = value; }
        public double ValueGreaterEQThan { get => _valueGreaterEQThan; set => _valueGreaterEQThan = value; }
        public DateTime D1 { get => _d1; set => _d1 = value; }
        public DateTime D2 { get => _d2; set => _d2 = value; }
    }
}