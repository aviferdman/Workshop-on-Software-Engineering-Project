using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;

namespace TradingSystem.Business.Market
{
    public class Discount
    {
        private Guid _id;
        private ICollection<IRule> _rules;
        private IDiscountCalculator _calc;

        public Discount(IDiscountCalculator calc)
        {
            Id = Guid.NewGuid();
            _rules = new HashSet<IRule>();
            Calc = calc;
        }

        public Guid Id { get => _id; set => _id = value; }
        public IDiscountCalculator Calc { get => _calc; set => _calc = value; }

        public void AddRule(IRule rule)
        {
            _rules.Add(rule);
        }

        public void RemoveRule(IRule rule)
        {
            _rules.Remove(rule);
        }

        public bool Available(IShoppingBasket shoppingBasket)
        {
            bool isLegal = true;
            foreach (IRule rule in _rules)
            {
                isLegal = isLegal && rule.Check(shoppingBasket);
            }
            return isLegal;
        }
        public double ApplyDiscounts(IShoppingBasket shoppingBasket)
        {
            bool isLegal = Available(shoppingBasket); 
            if (isLegal)    //all conditions are met, activate discount
            {
                return _calc.CalcDiscount(shoppingBasket);
            }
            //otherwise return 0 means no discount
            return 0;
        }

        public double XorHelper(IShoppingBasket shoppingBasket, Discount d1, Discount d2, bool decide)
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

        public Discount Xor(Discount d, bool decide)
        {
            Func<IShoppingBasket, double> f = new Func<IShoppingBasket, double>
                ((IShoppingBasket shoppingBasket) => XorHelper(shoppingBasket, this, d, decide)
                );

            return new Discount(new DiscountCalculator(f));
        }

        public Discount And(ICollection<IRule> additionalRules)
        {
            Discount discount = new Discount(new DiscountCalculator(_calc));
            //apply my rules
            foreach (var r in _rules)
            {
                discount.AddRule(r);
            }
            //apply additional rules
            foreach(var r in additionalRules)
            {
                discount.AddRule(r);
            }
            return discount;
        }

        private double OrHelper(IShoppingBasket shoppingBasket, Discount discount, ICollection<IRule> additionalRules)
        {
            bool isLegal = true;
            foreach (IRule rule in additionalRules)
            {
                isLegal = isLegal && rule.Check(shoppingBasket);
            }

            //either the discount rules available or the additional rules available
            if (discount.Available(shoppingBasket) || isLegal)
            {
                return discount.Calc.CalcDiscount(shoppingBasket);
            }

            //means no discount is available
            return 0;
        }

        public Discount Or (ICollection<IRule> additionalRules)
        {
            Func<IShoppingBasket, double> f = new Func<IShoppingBasket, double>
                ((IShoppingBasket shoppingBasket) => OrHelper(shoppingBasket, this, additionalRules)
                );

            return new Discount(new DiscountCalculator(f));

        }
    }
}
