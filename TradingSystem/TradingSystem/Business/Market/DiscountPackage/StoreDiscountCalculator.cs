﻿using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StorePackage.DiscountPackage
{
    public class StoreDiscountCalculator : IDiscountCalculator
    {
        IDiscountCalculator discountCalculator;
        public StoreDiscountCalculator(double percent)
        {
            Func<IShoppingBasket, double> f = new Func<IShoppingBasket, double>((IShoppingBasket basket) => Calc(basket, percent));
            discountCalculator = new DiscountCalculator(f);
        }

        private double Calc(IShoppingBasket basket, double percent)
        {
            double discount = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                discount += quantity * product.Price * percent;
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
