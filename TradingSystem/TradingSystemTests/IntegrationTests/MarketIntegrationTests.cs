using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class MarketIntegrationTests
    {
        private MarketUsers m = MarketUsers.Instance;
        private MarketStores marketStores = MarketStores.Instance;
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductSuccess()
        {
            string username=m.AddGuest();
            User u = m.GetUserByUserName(username);
            ShoppingCart cart = new ShoppingCart();
            u.ShoppingCart = cart;
            Product p = new Product("lala", 8,50, 500);
            Store s = new Store("lalali", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product added to shopping basket",m.AddProductToCart(username, p.Id, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            ShoppingCart cart = new ShoppingCart();
            u.ShoppingCart = cart;
            Product p = new Product("llll", 8, 50, 500);
            Assert.AreEqual("product doesn't exist", m.AddProductToCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductFail2()
        {
            Assert.AreEqual("user doesn't exist", m.AddProductToCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductFail3()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username); 
            ShoppingCart cart = new ShoppingCart();
            u.ShoppingCart = cart;
            Product p = new Product("lala2", 8, 50, 500);
            Store s = new Store("lalali2", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.AddProductToCart(username, p.Id, 500000));
        }


    }
}
