using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Discount
    {
        private Guid _id;
        private ICollection<IRule> _rules;
        private double _discount;

        public Discount(double discount)
        {
            Id = Guid.NewGuid();
            _rules = new HashSet<IRule>();
            DiscountValue = discount;
        }



        public Guid Id { get => _id; set => _id = value; }
        public double DiscountValue { get => _discount; set => _discount = value; }

        public void AddRule(IRule rule)
        {
            _rules.Add(rule);
        }

        public void RemoveRule(IRule rule)
        {
            _rules.Remove(rule);
        }

        public double ApplyDiscounts(Dictionary<Product, int> product_quantity)
        {
            bool isLegal = true;
            foreach (IRule rule in _rules)
            {
                isLegal = isLegal && rule.Check(product_quantity);
            }
            if (isLegal)    //all conditions are met, activate discount
            {
                return DiscountValue;
            }
            //otherwise return 0 means no discount
            return 0;
        }
    }
}
