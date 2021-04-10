using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class MarketTests
    {
        private Market m = Market.Instance;
        /// test for function :<see cref="TradingSystem.Business.Market.Market.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        public void AddProductSuccess()
        {
            string username=m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock <IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala", 8,50, 500);
            Store s = new Store("lalali", null, null);
            s.Products.TryAdd("lala", p);
            m.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product added to shopping basket",m.AddProductToCart(username, p.Id,p.Name, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.Market.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        public void AddProductFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("llll", 8, 50, 500);
            Assert.AreEqual("product doesn't exist", m.AddProductToCart(username, p.Id, p.Name, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Market.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        public void AddProductFail2()
        {
            Assert.AreEqual("user doesn't exist", m.AddProductToCart("lala", Guid.NewGuid(),"lala", 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Market.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        public void AddProductFail3()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala2", 8, 50, 500);
            Store s = new Store("lalali2", null, null);
            s.Products.TryAdd("lala2", p);
            m.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.AddProductToCart(username, p.Id, p.Name, 500000));
        }
    }
}
