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
            foreach (Product p in basket.GetProducts())
            {
                discount += p.Quantity * p.Price * percent;
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