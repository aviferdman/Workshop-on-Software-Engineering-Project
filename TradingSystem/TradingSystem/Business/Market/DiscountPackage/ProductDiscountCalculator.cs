using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class ProductDiscountCalculator : IDiscountCalculator
    {
        IDiscountCalculator discountCalculator;
        public ProductDiscountCalculator(Guid productId, double percent)
        {
            Func<IShoppingBasket, double> f = new Func<IShoppingBasket, double>((IShoppingBasket basket) => Calc(basket, productId, percent));
            discountCalculator = new DiscountCalculator(f);
        }

        private double Calc(IShoppingBasket basket, Guid productId, double percent)
        {
            double discount = 0;
            foreach (Product p in basket.GetProducts())
            {
                if (p.Id.Equals(productId))
                {
                    discount += p.Quantity * p.Price * percent;
                }
            }
            return discount;
        }

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Add(otherDiscountCalc);
        }

        public double CalcDiscount(IShoppingBasket shoppingBasket)
        {
            return discountCalculator.CalcDiscount(shoppingBasket);
        }

        public Func<IShoppingBasket, double> GetFunction()
        {
            return discountCalculator.GetFunction();
        }

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Max(otherDiscountCalc);
        }
    }
}
