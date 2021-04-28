using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class AddRemoveEditProductTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;
        ProductData product1;

        public AddRemoveEditProductTests()
        {
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("founder", "123", null, null);
            marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("manager", "123", null, null);
            marketUsers.AddMember("manager", "123", guestName);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            store = market.CreateStore("testStore", "founder", bankAccount, address);
            market.makeManager("manager", store.Id, "founder");
            product1 = new ProductData("1", 10, 10, 10, "c");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckValidAddProduct()
        {
            Result<Product> result = market.AddProduct(product1, store.Id, "founder");
            Assert.IsFalse(result.IsErr);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductUnauthorizedUser()
        {
            Result<Product> result = market.AddProduct(product1, store.Id, "no one");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidPrice()
        {
            ProductData product2 = new ProductData("1", 10, 10, -10, "category");
            Result<Product> result = market.AddProduct(product2, store.Id, "founder");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AddProduct(ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidName()
        {
            ProductData product2 = new ProductData("", 10, 10, 10, "category");
            Result<Product> result = market.AddProduct(product2, store.Id, "founder");
            Assert.IsTrue(result.IsErr);
            Assert.AreEqual(result.Mess, "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckValidRemoveProduct()
        {
            Product p = new Product(product1);
            store.Products.TryAdd(p.Id, p);
            Assert.AreEqual(market.RemoveProduct(p.Id, store.Id, "founder"), "Product removed");
            Assert.IsFalse(store.Products.TryRemove(p.Id, out _));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidUser()
        {
            Product p = new Product(product1);
            store.Products.TryAdd(p.Id, p);
            Assert.AreEqual(market.RemoveProduct(p.Id, store.Id, "no one"), "Invalid user");
            Assert.IsTrue(store.Products.TryRemove(p.Id, out _));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveProduct(Guid, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidPermission()
        {
            Product p = new Product(product1);
            store.Products.TryAdd(p.Id, p);
            Assert.AreEqual(market.RemoveProduct(p.Id, store.Id, "manager"), "No permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckValidEditProduct()
        {
            Product p1 = new Product(product1);
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            store.Products.TryAdd(p1.Id, p1);
            Assert.AreEqual(market.EditProduct(p1.Id, p2, store.Id, "founder"), "Product edited");
            Assert.IsTrue(store.Products.TryRemove(p1.Id, out Product p));
            Assert.AreEqual(p.Price, 20);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditUnavailablwProduct()
        {
            Product p1 = new Product(product1);
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            Assert.AreEqual(market.EditProduct(p1.Id, p2, store.Id, "founder"), "Product not in the store");
            Assert.IsFalse(store.Products.ContainsKey(p1.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.EditProduct(Guid, ProductData, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditNoPermission()
        {
            Product p1 = new Product(product1);
            ProductData p2 = new ProductData("1", 10, 10, 20, "category");
            store.Products.TryAdd(p1.Id, p1);
            Assert.AreEqual(market.EditProduct(p1.Id, p2, store.Id, "manager"), "No permission");
            Assert.IsTrue(store.Products.TryRemove(p1.Id, out Product p));
            Assert.AreEqual(p.Price, 10);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
