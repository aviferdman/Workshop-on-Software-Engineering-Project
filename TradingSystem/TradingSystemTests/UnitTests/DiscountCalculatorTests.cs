using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.DAL;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    public class DiscountCalculatorTests
    {
        private ShoppingBasket shoppingBasket;

        public DiscountCalculatorTests()
        {
            User u = new User();
            Store store = new Store("teststore", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.DiscountCalculator.CalcDiscount(IShoppingBasket)"/>
        [TestMethod]
        public void CheckCalcDiscount()
        {
            IDiscountCalculator discountCalculator1 = new DiscountCalculator(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 15));
            Assert.AreEqual(15, discountCalculator1.CalcDiscount(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.DiscountCalculator.Add(IDiscountCalculator)"/>
        [TestMethod]
        public void CheckAdd()
        {
            IDiscountCalculator discountCalculator1 = new DiscountCalculator(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 15));
            IDiscountCalculator discountCalculator2 = new DiscountCalculator(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 20));
            IDiscountCalculator discountCalculator3 = discountCalculator1.Add(discountCalculator2);
            Assert.AreEqual(35, discountCalculator3.CalcDiscount(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.DiscountCalculator.Max(IDiscountCalculator)"/>
        [TestMethod]
        public void CheckMax()
        {
            IDiscountCalculator discountCalculator1 = new DiscountCalculator(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 15));
            IDiscountCalculator discountCalculator2 = new DiscountCalculator(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 20));
            IDiscountCalculator discountCalculator3 = discountCalculator1.Max(discountCalculator2);
            Assert.AreEqual(20, discountCalculator3.CalcDiscount(shoppingBasket));
        }
    }
}
