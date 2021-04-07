using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
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
            return;
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
    }
}
