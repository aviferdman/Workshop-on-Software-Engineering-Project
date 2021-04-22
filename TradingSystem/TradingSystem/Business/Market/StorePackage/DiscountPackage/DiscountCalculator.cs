using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private Func<IShoppingBasket, double> _f;
        public DiscountCalculator(Func<IShoppingBasket, double> f)
        {
            F = f;
        }

        public DiscountCalculator(IDiscountCalculator calc)
        {
            this.F = new Func<IShoppingBasket, double>(calc.GetFunction());
        }

        public Func<IShoppingBasket, double> F { get => _f; set => _f = value; }

        public double CalcDiscount(IShoppingBasket shoppingBasket)
        {
            return F(shoppingBasket);
        }

        //takes two discount values and activates the function
        private double CompositeHelper(double discount1, double discount2, Func<double, double, double> compositeFunction)
        {
            return compositeFunction(discount1, discount2);
        }

        public IDiscountCalculator Max(IDiscountCalculator otherDiscountCalc)
        {
            Func<IShoppingBasket, double> newF = new Func<IShoppingBasket, double>((IShoppingBasket shoppingBasket) => 
                                                    CompositeHelper(this.CalcDiscount(shoppingBasket), otherDiscountCalc.CalcDiscount(shoppingBasket), (double d1, double d2) => Math.Max(d1, d2)));
            return new DiscountCalculator(newF);
        }

        public IDiscountCalculator Add(IDiscountCalculator otherDiscountCalc)
        {
            Func<IShoppingBasket, double> newF = new Func<IShoppingBasket, double>((IShoppingBasket shoppingBasket) =>
                                                    CompositeHelper(this.CalcDiscount(shoppingBasket), otherDiscountCalc.CalcDiscount(shoppingBasket), (double d1, double d2) => d1 + d2));
            return new DiscountCalculator(newF);
        }

        public Func<IShoppingBasket, double> GetFunction()
        {
            return F;
        }
    }
}
