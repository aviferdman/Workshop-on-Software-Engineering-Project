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
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    public class ProductsTest
    {
        Product product1;
        static Address address;
        static CreditCard card;
        Store store;
        Founder founder;
        Manager manager;

        public ProductsTest()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            product1 = new Product(new Guid(), "1", 10, 10, 10, "category");
            address = new Address("1", "1", "1", "1", "1");
            card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = new Store("testStore", card, address);
            MemberState ms = new MemberState("founder");
            founder = Founder.makeFounder(ms, store);
            Mock<Manager> imanager = new Mock<Manager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Founder = founder;
            store.Managers.Add(manager);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckValidAddProduct()
        {
            String res = await store.AddProduct(product1, "founder");
            Assert.AreEqual(res, "Product added");
            Assert.IsTrue(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductInvalidName()
        {
            Product product2 = new Product(new Guid(), "", 10, 10, 10, "category");
            String res = await store.AddProduct(product2, "founder");
            Assert.AreEqual(res, "Invalid product");
            Assert.IsFalse(store.Products.Contains(product2));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductInvalidPrice()
        {
            Product product2 = new Product(new Guid(), "2", 10, 10, -10, "category");
            String res = await store.AddProduct(product2, "founder");
            Assert.AreEqual(res, "Invalid product");
            Assert.IsFalse(store.Products.Contains(product2));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductNoPermission()
        {
            String res = await store.AddProduct(product1, "manager");
            Assert.AreEqual(res, "No permission");
            Assert.IsFalse(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public async Task CheckAddProductInvalidUser()
        {
            String res = await store.AddProduct(product1, "no one");
            Assert.AreEqual(res, "Invalid user");
            Assert.IsFalse(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckValidRemoveProduct()
        {
            store.Products.Add(product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "founder"), "Product removed");
            Assert.IsFalse(store.Products.Remove(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidUser()
        {;
            store.Products.Add(product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "no one"), "Invalid user");
            Assert.IsTrue(store.Products.Remove(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidPermission()
        {
            store.Products.Add(product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "manager"), "No permission");
            Assert.IsTrue(store.Products.Remove(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckValidEditProduct()
        {
            Product product2 = new Product(new Guid(), "1", 10, 10, 20, "category");
            store.Products.Add(product1);
            String res = await store.EditProduct(product1.Id, product2, "founder");
            Assert.AreEqual(res, "Product edited");
            Product product = store.GetProduct(product1.Id);
            Assert.IsTrue(store.Products.Remove(product1));
            Assert.AreEqual(product.Price, 20);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckEditUnavailablwProduct()
        {
            String res = await store.EditProduct(product1.Id, product1, "founder");
            Assert.AreEqual(res, "Product not in the store");
            Assert.IsFalse(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public async Task CheckEditNoPermission()
        {
            Product product2 = new Product(new Guid(), "1", 10, 10, 20, "category");
            store.Products.Add(product1);
            String res = await store.EditProduct(product1.Id, product2, "manager");
            Assert.AreEqual(res, "No permission");
            Product product = store.GetProduct(product1.Id);
            Assert.IsTrue(store.Products.Remove(product1));
            Assert.AreEqual(product.Price, 10);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            store = null;
        }
    }
}
