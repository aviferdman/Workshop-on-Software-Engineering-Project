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
            Assert.IsTrue(cart.GetShoppingBasket(s).GetProducts().Contains(p));
            Assert.AreEqual(cart.GetShoppingBasket(s).GetProductQuantity(p), 5);
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
            foreach(IShoppingBasket b in cart.Store_shoppingBasket.Values)
            {
                Assert.IsFalse(b.GetProducts().Contains(p));
            }
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
            Assert.IsFalse(cart.GetShoppingBasket(s).GetProducts().Contains(p));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartSuccess()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("lala3", 8, 50, 500);
            Store s = new Store("lalalil55", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            u.ShoppingCart.GetShoppingBasket(s).addProduct(p, 4);
            Assert.AreEqual("product removed from shopping basket", m.RemoveProductFromCart(username, p.Id));
            Assert.IsFalse(u.ShoppingCart.GetShoppingBasket(s).GetProducts().Contains(p));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("lala3", 8, 50, 500);
            Store s = new Store("lalalil55", null, null);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product doesn't exist", m.RemoveProductFromCart(username, p.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail2()
        {
            Assert.AreEqual("user doesn't exist", m.RemoveProductFromCart("11111", Guid.NewGuid()));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail3()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("lala3", 8, 50, 500);
            Store s = new Store("lalalil55", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product isn't in basket", m.RemoveProductFromCart(username, p.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartSuccess()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("lala8", 8, 50, 500);
            Store s = new Store("lalali80", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            u.ShoppingCart.GetShoppingBasket(s).addProduct(p, 4);
            Assert.AreEqual("product updated", m.ChangeProductQuanInCart(username, p.Id, 5));
            Assert.IsTrue(u.ShoppingCart.GetShoppingBasket(s).GetProducts().Contains(p));
            Assert.AreEqual(u.ShoppingCart.GetShoppingBasket(s).GetProductQuantity(p), 5);
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("llll8", 8, 50, 500);
            Assert.AreEqual("product doesn't exist", m.ChangeProductQuanInCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail2()
        {
            Assert.AreEqual("user doesn't exist", m.ChangeProductQuanInCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail3()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Product p = new Product("lala70", 8, 50, 500);
            Store s = new Store("lalali70", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            u.ShoppingCart.GetShoppingBasket(s).addProduct(p, 4);
            Assert.AreEqual("product's quantity is insufficient", m.ChangeProductQuanInCart(username, p.Id, 500000));
            Assert.IsTrue(u.ShoppingCart.GetShoppingBasket(s).GetProducts().Contains(p));
            Assert.AreEqual(u.ShoppingCart.GetShoppingBasket(s).GetProductQuantity(p), 4);
        }



    }
}
