using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class ShoppingBasketTests
    {
        private ShoppingCart shoppingCart;
        private IStore store;
        private ShoppingBasket shoppingBasket;

        public ShoppingBasketTests()
        {
            this.shoppingCart = new ShoppingCart();
            this.store = new Store("tets", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(shoppingCart, store);
        }

        //START UNIT TESTING

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingBasket.CalcCost()"/>
        [TestMethod]
        public void GetStoreHistoryWithoutPermission()
        {
            Product p1 = new Product(100, 100, 100);
            Product p2 = new Product(200, 200, 200);
            Product p3 = new Product(300, 300, 300);
            shoppingBasket.UpdateProduct(p1, 5);
            shoppingBasket.UpdateProduct(p2, 5);
            shoppingBasket.UpdateProduct(p3, 5);
            Assert.AreEqual(3000, shoppingBasket.CalcCost());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingBasket.IsEmpty()"/>
        [TestMethod]
        public void IsEmptyWithoutAnyProducts()
        {
            Assert.AreEqual(true, shoppingBasket.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingBasket.IsEmpty()"/>
        [TestMethod]
        public void IsEmptyWithProductsWithZeroQuantity()
        {
            Product p1 = new Product(100, 100, 100);
            Product p2 = new Product(200, 200, 200);
            Product p3 = new Product(300, 300, 300);
            shoppingBasket.UpdateProduct(p1, 0);
            shoppingBasket.UpdateProduct(p2, 0);
            shoppingBasket.UpdateProduct(p3, 0);
            Assert.AreEqual(true, shoppingBasket.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingBasket.IsEmpty()"/>
        [TestMethod]
        public void IsEmptyWithProductsWithPositiveQuantity()
        {
            Product p1 = new Product(100, 100, 100);
            Product p2 = new Product(200, 200, 200);
            Product p3 = new Product(300, 300, 300);
            shoppingBasket.UpdateProduct(p1, 5);
            shoppingBasket.UpdateProduct(p2, 5);
            shoppingBasket.UpdateProduct(p3, 0);
            Assert.AreEqual(false, shoppingBasket.IsEmpty());
        }


        [TestCleanup]
        public void DeleteAll()
        {
            this.shoppingCart = new ShoppingCart();
            this.store = new Store("tets", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(shoppingCart, store);
            Transaction.Instance.DeleteAllTests();
        }

        //END UNIT TESTING

    }
}
