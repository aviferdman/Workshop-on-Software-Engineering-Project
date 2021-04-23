using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Interfaces
{
    public interface IDiscountCalculator
    {
        public double CalcDiscount(IShoppingBasket shoppingBasket);

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc);

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc);
        Func<IShoppingBasket, double> GetFunction();
    }
}
