using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
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
            product1 = new Product("1", 10, 10, 10, "category");
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
        public void CheckValidAddProduct()
        {
            Assert.AreEqual(store.AddProductAsync(product1, "founder"), "Product added");
            Assert.IsTrue(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidName()
        {
            Product product2 = new Product("", 10, 10, 10, "category");
            Assert.AreEqual(store.AddProductAsync(product2, "founder"), "Invalid product");
            Assert.IsFalse(store.Products.Contains(product2));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidPrice()
        {
            Product product2 = new Product("2", 10, 10, -10, "category");
            Assert.AreEqual(store.AddProductAsync(product2, "founder"), "Invalid product");
            Assert.IsFalse(store.Products.Contains(product2));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductNoPermission()
        {
            Assert.AreEqual(store.AddProductAsync(product1, "manager"), "No permission");
            Assert.IsFalse(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidUser()
        {
            Assert.AreEqual(store.AddProductAsync(product1, "no one"), "Invalid user");
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
        public void CheckValidEditProduct()
        {
            Product product2 = new Product("1", 10, 10, 20, "category");
            store.Products.Add(product1);
            Assert.AreEqual(store.EditProductAsync(product1.Id, product2, "founder"), "Product edited");
            Product product = store.GetProduct(product1.Id);
            Assert.IsTrue(store.Products.Remove(product1));
            Assert.AreEqual(product.Price, 20);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditUnavailablwProduct()
        {
            Assert.AreEqual(store.EditProductAsync(product1.Id, product1, "founder"), "Product not in the store");
            Assert.IsFalse(store.Products.Contains(product1));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditNoPermission()
        {
            Product product2 = new Product("1", 10, 10, 20, "category");
            store.Products.Add(product1);
            Assert.AreEqual(store.EditProduct(product1.Id, product2, "manager"), "No permission");
            Product product = store.GetProduct(product1.Id);
            Assert.IsTrue(store.Products.Remove(product1));
            Assert.AreEqual(product.Price, 10);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
