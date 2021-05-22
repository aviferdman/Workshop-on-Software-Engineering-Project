using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.DAL;

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
            ProxyMarketContext.Instance.IsDebug = true;
            string username=m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock <ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(Task.FromResult(bask.Object));
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(Task.FromResult("product added to shopping basket"));
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala", 8,50, 500, "category");
            Store s = new Store("lalali", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product added to shopping basket",m.AddProductToCart(username, p.Id, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail1()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(Task.FromResult(bask.Object));
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(Task.FromResult("product added to shopping basket"));
            u.ShoppingCart = cart.Object;
            Product p = new Product("llll", 8, 50, 500, "category");
            Assert.AreEqual("product doesn't exist", m.AddProductToCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail2()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            Assert.AreEqual("user doesn't exist", m.AddProductToCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public void AddProductInCartFail3()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.GetShoppingBasket(It.IsAny<Store>())).Returns(Task.FromResult(bask.Object));
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(Task.FromResult("product added to shopping basket"));
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala2", 8, 50, 500, "category");
            Store s = new Store("lalali2", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.AddProductToCart(username, p.Id, 500000));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartSuccess()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(true);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
            Store s = new Store("lalalil55", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product removed from shopping basket",m.RemoveProductFromCart(username, p.Id));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail1()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(true);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
            Store s = new Store("lalalil55", null, null);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product doesn't exist", m.RemoveProductFromCart(username, p.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail2()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            Assert.AreEqual("user doesn't exist", m.RemoveProductFromCart("11111", Guid.NewGuid()));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail3()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.RemoveProduct(It.IsAny<Product>())).Returns(false);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala3", 8, 50, 500, "category");
            Store s = new Store("lalalil55", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product isn't in basket", m.RemoveProductFromCart(username, p.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartSuccess()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.TryUpdateProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(true);
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala8", 8, 50, 500, "category");
            Store s = new Store("lalali80", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product updated", m.ChangeProductQuanInCart(username, p.Id, 5));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail1()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(Task.FromResult("product added to shopping basket"));
            u.ShoppingCart = cart.Object;
            Product p = new Product("llll8", 8, 50, 500, "category");
            Assert.AreEqual("product doesn't exist", m.ChangeProductQuanInCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail2()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            Assert.AreEqual("user doesn't exist", m.ChangeProductQuanInCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail3()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
            Mock<ShoppingCart> cart = new Mock<ShoppingCart>();
            Mock<ShoppingBasket> bask = new Mock<ShoppingBasket>();
            cart.Setup(c => c.TryGetShoppingBasket(It.IsAny<Store>())).Returns(bask.Object);
            bask.Setup(b => b.addProduct(It.IsAny<Product>(), It.IsAny<int>())).Returns(Task.FromResult("product added to shopping basket"));
            u.ShoppingCart = cart.Object;
            Product p = new Product("lala70", 8, 50, 500, "category");
            Store s = new Store("lalali70", null, null);
            s.Products.Add(p);
            marketStores.LoadedStores.TryAdd(s.GetId(), s);
            Assert.AreEqual("product's quantity is insufficient", m.ChangeProductQuanInCart(username, p.Id, 500000));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.viewShoppingCart(string)"/>
        [TestMethod]
        [TestCategory("uc6")]
        public void ViewShoppingCartTestSuccess()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            string username = m.AddGuest();
            User u = m.GetUserByUserName(username);
           
            Assert.AreEqual(u.ShoppingCart, m.viewShoppingCart(username));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.viewShoppingCart(string)"/>
        [TestMethod]
        [TestCategory("uc6")]
        public void ViewShoppingCartTestFail()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            Assert.AreEqual(null, m.viewShoppingCart("lala"));
        }

    }
}
