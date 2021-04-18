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
        private MarketUsers m = MarketUsers.Instance;
        private MarketStores marketStores = MarketStores.Instance;
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartSuccess()
        {
            string username=m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock <IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala", 8,50, 500, "category");
            Store s = new Store("lalali", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product added to shopping basket",m.AddProductToCart(username, p.Id, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("llll", 8, 50, 500, "category");
            Assert.AreEqual("product doesn't exist", m.AddProductToCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail2()
        {
            Assert.AreEqual("user doesn't exist", m.AddProductToCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail3()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala2", 8, 50, 500, "category");
            Store s = new Store("lalali2", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.AddProductToCart(username, p.Id, 500000));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartSuccess()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(true);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
            Store s = new Store("lalalil55", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product removed from shopping basket",m.RemoveProductFromCart(username, p.Id));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(true);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
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
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(false);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
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
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.UpdateProduct(It.IsAny<Product>(), It.IsAny<int>()));
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala8", 8, 50, 500, "category");
            Store s = new Store("lalali80", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product updated", m.ChangeProductQuanInCart(username, p.Id, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail1()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("llll8", 8, 50, 500, "category");
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
            Mock<IShoppingCart> cart = new Mock<IShoppingCart>();
            Mock<IShoppingBasket> bask = new Mock<IShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns("product added to shopping basket");
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala70", 8, 50, 500, "category");
            Store s = new Store("lalali70", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.ChangeProductQuanInCart(username, p.Id, 500000));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.viewShoppingCart(string)"/>
        [TestMethod]
        [TestCategory("uc6")]
        public void ViewShoppingCartTestSuccess()
        {
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
           
            Assert.AreEqual(u.ShoppingCart, m.viewShoppingCart(username));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.viewShoppingCart(string)"/>
        [TestMethod]
        [TestCategory("uc6")]
        public void ViewShoppingCartTestFail()
        {

            Assert.AreEqual(null, m.viewShoppingCart("lala"));
        }

    }
}
