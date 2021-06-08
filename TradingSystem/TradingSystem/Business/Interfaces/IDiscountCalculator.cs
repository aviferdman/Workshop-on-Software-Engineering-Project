using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;

namespace TradingSystem.Business.Interfaces
{
    public interface IDiscountCalculator
    {
        public DiscountOfProducts CalcDiscount(ShoppingBasket shoppingBasket);

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc);

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc);
        Func<ShoppingBasket, DiscountOfProducts> GetFunction();
    }
}
