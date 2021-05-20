using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class CategoryDiscountCalculator : IDiscountCalculator
    {
        IDiscountCalculator discountCalculator;
        public CategoryDiscountCalculator(string category, double percent)
        {
            Func<ShoppingBasket, double> f = new Func<ShoppingBasket, double>((ShoppingBasket basket) => Calc(basket, category, percent));
            discountCalculator = new DiscountCalculator(f);
        }

        private double Calc(ShoppingBasket basket, string category, double percent)
        {
            double discount = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                if (product.Category.Equals(category))
                {
                    discount += quantity * product.Price * percent;
                }
            }
            return discount;
        }

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Add(otherDiscountCalc);
        }

        public double CalcDiscount(ShoppingBasket shoppingBasket)
        {
            return discountCalculator.CalcDiscount(shoppingBasket);
        }

        public Func<ShoppingBasket, double> GetFunction()
        {
            return discountCalculator.GetFunction();
        }

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Max(otherDiscountCalc);
        }

    }
}
