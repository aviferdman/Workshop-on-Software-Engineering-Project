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

        private IDiscountCalculator _calc;

        public Discount(IDiscountCalculator calc)
        {
            Id = Guid.NewGuid();
            Calc = calc;
        }

        public Guid Id { get => _id; set => _id = value; }
        public IDiscountCalculator Calc { get => _calc; set => _calc = value; }

        public virtual double ApplyDiscounts(IShoppingBasket shoppingBasket)
        {
            return _calc.CalcDiscount(shoppingBasket);
        }
        public virtual IRule GetRule()
        {
            return null;
        }
    }
}
