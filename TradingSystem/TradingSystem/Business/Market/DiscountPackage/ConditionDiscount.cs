using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class ConditionDiscount : Discount
    {
        private IRule _rule;
        public ConditionDiscount(IDiscountCalculator calc): base(calc)
        {
            
        }
        public override IRule GetRule()
        {
            return _rule;
        }
        public void AddRule(IRule rule)
        {
            _rule = rule;
        }

        public void RemoveRule(IRule rule)
        {
            _rule = null;
        }
        public override double ApplyDiscounts(IShoppingBasket shoppingBasket)
        {
            if (Available(shoppingBasket))
            {
                return base.Calc.CalcDiscount(shoppingBasket);
            }
            return 0;
        }

        public bool Available(IShoppingBasket shoppingBasket)
        {
            return _rule == null || _rule.Check(shoppingBasket);
        }
        public double XorHelper(IShoppingBasket shoppingBasket, ConditionDiscount d1, ConditionDiscount d2, bool decide)
        {
            bool available1 = d1.Available(shoppingBasket);
            bool available2 = d2.Available(shoppingBasket);
            //if both discounts are available decide by the 'decide' value
            if (available1 && available2)
            {
                if (decide)
                {
                    return d1.Calc.CalcDiscount(shoppingBasket);
                }
                else
                {
                    return d2.Calc.CalcDiscount(shoppingBasket);
                }
            }
            else if (available1)
            {
                return d1.Calc.CalcDiscount(shoppingBasket);
            }
            else
            {
                return d2.ApplyDiscounts(shoppingBasket);
            }
        }

        public ConditionDiscount Xor(ConditionDiscount d, bool decide)
        {
            Func<IShoppingBasket, double> f = new Func<IShoppingBasket, double>
                ((IShoppingBasket shoppingBasket) => XorHelper(shoppingBasket, this, d, decide)
                );

            return new ConditionDiscount(new DiscountCalculator(f));
        }

        public ConditionDiscount And(IRule additionalRule)
        {
            ConditionDiscount discount = new ConditionDiscount(new DiscountCalculator(base.Calc));
            IRule andRule = Rule.AddTwoRules(_rule, additionalRule);
            discount.AddRule(andRule);
            return discount;
        }

        public ConditionDiscount Or(IRule additionalRule)
        {
            ConditionDiscount discount = new ConditionDiscount(new DiscountCalculator(base.Calc));
            IRule orRule = Rule.OrTwoRules(_rule, additionalRule);
            discount.AddRule(orRule);
            return discount;

        }
    }
}
