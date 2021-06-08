using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.DiscountPackage;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private Func<ShoppingBasket, DiscountOfProducts> _f;
        public DiscountCalculator(Func<ShoppingBasket, DiscountOfProducts> f)
        {
            F = f;
        }

        public DiscountCalculator(IDiscountCalculator calc)
        {
            this.F = new Func<ShoppingBasket, DiscountOfProducts>(calc.GetFunction());
        }

        public Func<ShoppingBasket, DiscountOfProducts> F { get => _f; set => _f = value; }

        public virtual DiscountOfProducts CalcDiscount(ShoppingBasket shoppingBasket)
        {
            return F(shoppingBasket);
        }

        //takes two discount values and activates the function
        //private DiscountOfProducts CompositeHelper(double discount1, double discount2, Func<double, double, double> compositeFunction)
        //{
        //    return compositeFunction(discount1, discount2);
        //}



        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc)
        {
            Func<ShoppingBasket, DiscountOfProducts> newF = new Func<ShoppingBasket, DiscountOfProducts>((ShoppingBasket shoppingBasket) =>
                                                    //CompositeHelper(this.CalcDiscount(shoppingBasket), otherDiscountCalc.CalcDiscount(shoppingBasket), (double d1, double d2) => Math.Max(d1, d2)));
                                                    MaxOfDiscounts(this, otherDiscountCalc, shoppingBasket));
            return new DiscountCalculator(newF);
        }

        private DiscountOfProducts MaxOfDiscounts(DiscountCalculator discountCalculator, IDiscountCalculator otherDiscountCalc, ShoppingBasket shoppingBasket)
        {
            var d1 = discountCalculator.CalcDiscount(shoppingBasket);
            var d2 = otherDiscountCalc.CalcDiscount(shoppingBasket);
            if (d1.Discount > d2.Discount)
            {
                return d1;
            }
            return d2;
        }

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc)
        {
            Func<ShoppingBasket, DiscountOfProducts> newF = new Func<ShoppingBasket, DiscountOfProducts>((ShoppingBasket shoppingBasket) =>
                                                    //CompositeHelper(this.CalcDiscount(shoppingBasket), otherDiscountCalc.CalcDiscount(shoppingBasket), (double d1, double d2) => d1 + d2));
                                                    AddHelper(this, otherDiscountCalc, shoppingBasket));

            return new DiscountCalculator(newF);
        }

        private DiscountOfProducts AddHelper(DiscountCalculator discountCalculator, IDiscountCalculator otherDiscountCalc, ShoppingBasket shoppingBasket)
        {
            var d1 = discountCalculator.CalcDiscount(shoppingBasket);
            var d2 = otherDiscountCalc.CalcDiscount(shoppingBasket);
            var ret = d1.AddDiscounts(d2.Discount, d2.Products);
            return ret;
        }

        public virtual Func<ShoppingBasket, DiscountOfProducts> GetFunction()
        {
            return F;
        }
    }
}
