using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.DiscountPackage;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class StoreDiscountCalculator : IDiscountCalculator
    {
        IDiscountCalculator discountCalculator;
        public StoreDiscountCalculator(double percent)
        {
            Func<ShoppingBasket, DiscountOfProducts> f = new Func<ShoppingBasket, DiscountOfProducts>((ShoppingBasket basket) => Calc(basket, percent));
            discountCalculator = new DiscountCalculator(f);
        }

        private DiscountOfProducts Calc(ShoppingBasket basket, double percent)
        {
            var ret = new DiscountOfProducts();
            double discount = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                discount += quantity * product.Price * percent;
                ret.Products.Add(product.Id, (1 - percent) * product.Price);
            }
            ret.Discount = discount;
            return ret;
        }

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Add(otherDiscountCalc);
        }

        public virtual DiscountOfProducts CalcDiscount(ShoppingBasket shoppingBasket)
        {
            return discountCalculator.CalcDiscount(shoppingBasket);
        }

        public virtual Func<ShoppingBasket, DiscountOfProducts> GetFunction()
        {
            return discountCalculator.GetFunction();
        }

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc)
        {
            return discountCalculator.Max(otherDiscountCalc);
        }
    }
}
