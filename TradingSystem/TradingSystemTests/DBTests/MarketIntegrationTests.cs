using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class MarketIntegrationTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;
        ProductData product1;
        public MarketIntegrationTests()
        {
            ProxyMarketContext.Instance.IsDebug = false;
        }
        [TestInitialize]
        public async Task Initialize()
        {
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager", "123", null);
            await marketUsers.AddMember("manager", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "founder", card, address);
            await market.makeManager("manager", store.Id, "founder");
            product1 = new ProductData("1", 10, 10, 10, "c");
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public async Task AddProductSuccess()
        {
            string username=marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            ShoppingCart cart = u.ShoppingCart;
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            Assert.AreEqual("product added to shopping basket", await marketUsers.AddProductToCart(username, result.Ret.Id, 5));
            ShoppingBasket b = await cart.GetShoppingBasket(store);
            Assert.IsTrue(b.GetProducts().Contains(result.Ret));
            Assert.AreEqual(b.GetProductQuantity(result.Ret), 5);
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public async Task AddProductFail1Async()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            ShoppingCart cart = u.ShoppingCart;
            Product p = new Product("llll", 8, 50, 500,"category");
            Assert.AreEqual("product doesn't exist", await marketUsers.AddProductToCart(username, p.Id, 5));
            foreach(ShoppingBasket b in cart.ShoppingBaskets)
            {
                Assert.IsFalse(b.GetProducts().Contains(p));
            }
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public async Task AddProductFail2Async()
        {
            Assert.AreEqual("user doesn't exist", await marketUsers.AddProductToCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddProductToCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc5")]
        public async Task AddProductFail3()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            ShoppingCart cart = u.ShoppingCart;
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            Assert.AreEqual("product's quantity is insufficient", await marketUsers.AddProductToCart(username, result.Ret.Id, 500000));
            ShoppingBasket b = await cart.GetShoppingBasket(store);
            Assert.IsFalse(b.GetProducts().Contains(result.Ret));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public async Task removeProductInCartSuccess()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            ShoppingBasket b = await u.ShoppingCart.GetShoppingBasket(store);
            await b.addProduct(result.Ret, 4);
            Assert.AreEqual("product removed from shopping basket", marketUsers.RemoveProductFromCart(username, result.Ret.Id));
            Assert.IsFalse(b.GetProducts().Contains(result.Ret));
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail1()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            Assert.AreEqual("product doesn't exist", marketUsers.RemoveProductFromCart(username, Guid.NewGuid()));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public void removeProductInCartFail2()
        {
            Assert.AreEqual("user doesn't exist", marketUsers.RemoveProductFromCart("11111", Guid.NewGuid()));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.RemoveProductFromCart(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc8")]
        public async Task removeProductInCartFail3Async()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username); 
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            Assert.AreEqual("product isn't in basket", marketUsers.RemoveProductFromCart(username, result.Ret.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public async Task updateProductInCartSuccess()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            ShoppingBasket b = await u.ShoppingCart.GetShoppingBasket(store);
            await b.addProduct(result.Ret, 4);
            Assert.AreEqual("product updated", marketUsers.ChangeProductQuanInCart(username, result.Ret.Id, 5));
            Assert.IsTrue(b.GetProducts().Contains(result.Ret));
            Assert.AreEqual(b.GetProductQuantity(result.Ret), 5);
        }
        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail1()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            Product p = new Product("llll8", 8, 50, 500, "category");
            Assert.AreEqual("product doesn't exist", marketUsers.ChangeProductQuanInCart(username, p.Id, 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public void updateProductInCartFail2()
        {
            Assert.AreEqual("user doesn't exist", marketUsers.ChangeProductQuanInCart("lala", Guid.NewGuid(), 5));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.ChangeProductQuanInCart(string, Guid, string, int)"/>
        [TestMethod]
        [TestCategory("uc9")]
        public async Task updateProductInCartFail3Async()
        {
            string username = marketUsers.AddGuest();
            User u = marketUsers.GetUserByUserName(username);
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            ShoppingBasket b = await u.ShoppingCart.GetShoppingBasket(store);
            await b.addProduct(result.Ret, 4);
            Assert.AreEqual("product's quantity is insufficient", marketUsers.ChangeProductQuanInCart(username, result.Ret.Id, 500000));
            Assert.IsTrue(b.GetProducts().Contains(result.Ret));
            Assert.AreEqual(b.GetProductQuantity(result.Ret), 4);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// no price range or rating
        /// category: vegan
        /// keyword: soy milk
        [TestMethod]
        [TestCategory("uc4")]
        public async Task findProductsuccess()
        {
            ProductData p = new ProductData("soy milk", 8, 50, 500, "vegan");
            Store s = store = await market.CreateStore("Vegan store12", "founder", null, null);
            Result<Product> result1 = await MarketStores.Instance.AddProduct(p, store.Id, "founder");
            ProductData p2 = new ProductData("soy milk diff", 8, 50, 500, "vegan");
            Store s2 = await market.CreateStore("vegan store 2", "founder", null, null);
            Result<Product> result2 = await MarketStores.Instance.AddProduct(p2, store.Id, "founder");
            ICollection<Product> ret= await market.findProducts("soy milk", -1, -1, -1, "vegan");
            Assert.IsTrue(ret.Contains(result1.Ret) && ret.Contains(result2.Ret) );
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// no price range or rating no category
        /// keyword: tofu
        [TestMethod]
        [TestCategory("uc4")]
        public async Task findProductsuccess2()
        {
            ProductData p = new ProductData("tofu", 8, 50, 500, "vegan");
            Store s = store = await market.CreateStore("vegan store3", "founder", null, null);
            Result<Product> result1 = await MarketStores.Instance.AddProduct(p, store.Id, "founder");
            ProductData p2 = new ProductData("hummus tofu", 8, 50, 500, "vegan");
            Store s2 = await market.CreateStore("vegan store 4", "founder", null, null);
            Result<Product> result2 = await MarketStores.Instance.AddProduct(p2, store.Id, "founder");
            ICollection<Product> ret =await market.findProducts("tofu", -1, -1, -1, null);
            Assert.IsTrue( ret.Count == 2);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// price range: 4-20
        /// or rating no category
        /// keyword: watermelon
        [TestMethod]
        [TestCategory("uc4")]
        public async Task findProductsuccess3()
        {
            ProductData p = new ProductData("watermelon exp", 8, 50, 500.0, "vegatables");
            Store s = store = await market.CreateStore("vegan store5", "founder", null, null);
            Result<Product> result1 = await MarketStores.Instance.AddProduct(p, store.Id, "founder");
            ProductData p2 = new ProductData("watermelon cheep", 8, 50, 5.0, "vegatables");
            Store s2 = await market.CreateStore("vegan store 6", "founder", null, null);
            Result<Product> result2 = await MarketStores.Instance.AddProduct(p2, store.Id, "founder");
            ICollection<Product> ret = await market.findProducts("watermelon", 4, 20, -1, null);
            Assert.IsTrue(ret.Contains(result2.Ret) && ret.Count==1);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        /// rating: 5
        /// price range no category
        /// keyword: cucumber
        [TestMethod]
        [TestCategory("uc4")]
        public async Task findProductsuccess4()
        {
            ProductData p = new ProductData("cucumber high rating", 8, 50, 500.0, "vegatables");
            p.rating = 5;
            Store s = store = await market.CreateStore("vegan store7", "founder", null, null);
            Result<Product> result1 = await MarketStores.Instance.AddProduct(p, store.Id, "founder");
            ProductData p2 = new ProductData("cucumber low rating", 8, 50, 5.0, "vegatables");
            p2.rating = 2;
            Store s2 = await market.CreateStore("vegan store 8", "founder", null, null);
            Result<Product> result2 = await MarketStores.Instance.AddProduct(p2, store.Id, "founder");
            ICollection<Product> ret = await market.findProducts("cucumber", -1, -1, 5, null);
            Assert.IsTrue(ret.Contains(result1.Ret) && ret.Count == 1);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.findProducts(string, int, int, int, string)"/>
        [TestMethod]
        [TestCategory("uc4")]
        public async Task findProductNoProductFound()
        {
            ICollection<Product> ret = await market.findProducts("nothing to see", -1, -1, 5, null);
            Assert.IsTrue( ret.Count==0);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            market.tearDown();
            marketUsers.tearDown();
            userManagement.tearDown();
            store = null;
        }

    }
}
