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
            ShoppingCart cart = new ShoppingCart(u);
            u.ShoppingCart = cart;
            Product p = new Product("lala", 8,50, 500, "category");
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
            ShoppingCart cart = new ShoppingCart(u);
            u.ShoppingCart = cart;
            Product p = new Product("llll", 8, 50, 500,"category");
            Assert.AreEqual("product doesn't exist", m.AddProductToCart(username, p.Id, 5));
            foreach(IShoppingBasket b in cart.ShoppingBaskets)
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
            ShoppingCart cart = new ShoppingCart(u);
            u.ShoppingCart = cart;
            Product p = new Product("lala2", 8, 50, 500, "category");
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
            Product p = new Product("lala3", 8, 50, 500, "category");
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
            Product p = new Product("lala8", 8, 50, 500, "category");
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
            Product p = new Product("lala70", 8, 50, 500, "category");
            Store s = new Store("lalali70", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.Stores.TryAdd(s.GetId(), s);
            u.ShoppingCart.GetShoppingBasket(s).addProduct(p, 4);
            Assert.AreEqual("product's quantity is insufficient", m.ChangeProductQuanInCart(username, p.Id, 500000));
            Assert.IsTrue(u.ShoppingCart.GetShoppingBasket(s).GetProducts().Contains(p));
            Assert.AreEqual(u.ShoppingCart.GetShoppingBasket(s).GetProductQuantity(p), 4);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// no price range or rating
        /// category: vegan
        /// keyword: soy milk
        [TestMethod]
        [TestCategory("uc4")]
        public void findProductsuccess()
        {
            Product p = new Product("soy milk", 8, 50, 500, "vegan");
            Store s = new Store("vegan store", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.addToCategory(p, "vegan");
            marketStores.Stores.TryAdd(s.GetId(), s);
            Product p2 = new Product("soy milk diff", 8, 50, 500, "vegan");
            Store s2 = new Store("vegan store 2", null, null);
            s2.Products.TryAdd(p2.Id, p2);
            marketStores.addToCategory(p2, "vegan");
            marketStores.Stores.TryAdd(s2.GetId(), s2);
            ICollection<Product> ret= marketStores.findProducts("soy milk", -1, -1, -1, "vegan");
            Assert.IsTrue(ret.Contains(p) && ret.Contains(p2) );
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// no price range or rating no category
        /// keyword: tofu
        [TestMethod]
        [TestCategory("uc4")]
        public void findProductsuccess2()
        {
            Product p = new Product("tofu", 8, 50, 500, "vegan");
            Store s = new Store("vegan store3", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.addToCategory(p, "vegan");
            marketStores.Stores.TryAdd(s.GetId(), s);
            Product p2 = new Product("hummus tofu", 8, 50, 500, "vegan");
            Store s2 = new Store("vegan store 4", null, null);
            s2.Products.TryAdd(p2.Id, p2);
            marketStores.addToCategory(p2, "vegan");
            marketStores.Stores.TryAdd(s2.GetId(), s2);
            ICollection<Product> ret = marketStores.findProducts("tofu", -1, -1, -1, null);
            Assert.IsTrue(ret.Contains(p) && ret.Contains(p2) && ret.Count == 2);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// price range: 4-20
        /// or rating no category
        /// keyword: watermelon
        [TestMethod]
        [TestCategory("uc4")]
        public void findProductsuccess3()
        {
            Product p = new Product("watermelon exp", 8, 50, 500.0, "vegatables");
            Store s = new Store("vegan store5", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.addToCategory(p, "vegatables");
            marketStores.Stores.TryAdd(s.GetId(), s);
            Product p2 = new Product("watermelon cheep", 8, 50, 5.0, "vegatables");
            marketStores.addToCategory(p2, "vegatables");
            Store s2 = new Store("vegan store 6", null, null);
            s2.Products.TryAdd(p2.Id, p2);
            marketStores.Stores.TryAdd(s2.GetId(), s2);
            ICollection<Product> ret = marketStores.findProducts("watermelon", 4, 20, -1, null);
            Assert.IsTrue(ret.Contains(p2)&& ret.Count==1);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// rating: 5
        /// price range no category
        /// keyword: cucumber
        [TestMethod]
        [TestCategory("uc4")]
        public void findProductsuccess4()
        {
            Product p = new Product("cucumber high rating", 8, 50, 500.0, "vegatables");
            p.Rating = 5;
            Store s = new Store("vegan store7", null, null);
            s.Products.TryAdd(p.Id, p);
            marketStores.addToCategory(p, "vegatables");
            marketStores.Stores.TryAdd(s.GetId(), s);
            Product p2 = new Product("cucumber low rating", 8, 50, 5.0, "vegatables");
            p2.Rating = 2;
            marketStores.addToCategory(p2, "vegatables");
            Store s2 = new Store("vegan store 8", null, null);
            s2.Products.TryAdd(p2.Id, p2);
            marketStores.Stores.TryAdd(s2.GetId(), s2);
            ICollection<Product> ret = marketStores.findProducts("cucumber", -1, -1, 5, null);
            Assert.IsTrue(ret.Contains(p) && ret.Count == 1);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        [TestMethod]
        [TestCategory("uc4")]
        public void findProductNoProductFound()
        {
            ICollection<Product> ret = marketStores.findProducts("nothing to see", -1, -1, 5, null);
            Assert.IsTrue( ret.Count==0);
        }

    }
}
