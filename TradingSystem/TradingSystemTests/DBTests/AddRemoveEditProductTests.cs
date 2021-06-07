using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class AddRemoveEditProductTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;
        ProductData product1;

        [TestInitialize]
        public async Task Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = false;
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager", "123",  null);
            await marketUsers.AddMember("manager", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "founder", card, address);
            await market.makeManager("manager", store.Id, "founder");
            product1 = new ProductData("1", 10, 10, 10, "c");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckValidAddProduct()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductUnauthorizedUser()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "no one");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductInvalidPrice()
        {
            ProductData product2 = new ProductData("1", 10, 10, -10, "category");
            Result<Product> result = await market.AddProduct(product2, store.Id, "founder");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductInvalidName()
        {
            ProductData product2 = new ProductData("", 10, 10, 10, "category");
            Result<Product> result = await market.AddProduct(product2, store.Id, "founder");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public async Task CheckValidRemoveProduct()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
            Assert.AreEqual(await market.RemoveProduct(result.Ret.Id, store.Id, "founder"), "Product removed");
            Assert.IsFalse(store.Products.Remove(result.Ret));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public async Task CheckRemoveProductInvalidUserAsync()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
            Assert.AreEqual(await market.RemoveProduct(result.Ret.Id, store.Id, "no one"), "Invalid user");
            Assert.IsTrue(store.Products.Remove(result.Ret));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public async Task CheckRemoveProductInvalidPermissionAsync()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
            Assert.AreEqual(await market.RemoveProduct(result.Ret.Id, store.Id, "manager"), "No permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckValidEditProductAsync()
        {
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            Assert.AreEqual(await market.EditProduct(result.Ret.Id, p2, store.Id, "founder"), "Product edited");
            Product p = store.GetProduct(result.Ret.id);
            Assert.IsTrue(store.Products.Remove(result.Ret));
            Assert.AreEqual(p.Price, 20);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckEditUnavailablwProductAsync()
        {
            Product p1 = new Product(product1);
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            Assert.AreEqual(await market.EditProduct(p1.Id, p2, store.Id, "founder"), "Product not in the store");
            Assert.IsFalse(store.Products.Contains(p1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckEditNoPermissionAsync()
        {
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            Result<Product> result = await market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
            Assert.AreEqual(await market.EditProduct(result.Ret.Id, p2, store.Id, "manager"), "No permission");
            Product p = store.GetProduct(result.Ret.Id);
            Assert.IsTrue(store.Products.Remove(result.Ret));
            Assert.AreEqual(p.Price, 10);
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
