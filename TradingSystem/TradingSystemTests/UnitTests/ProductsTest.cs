using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    class ProductsTest
    {
        Product product1;
        static Address address;
        static BankAccount bankAccount;
        Store store;
        Founder founder;
        IManager manager;

        public ProductsTest()
        {
            product1 = new Product("1", 10, 10, 10, "category");
            address = new Address("1", "1", "1", "1");
            bankAccount = new BankAccount(1000, 1000);
            store = new Store("testStore", bankAccount, address);
            MemberState ms = new MemberState("founder");
            founder = Founder.makeFounder(ms, store);
            Mock<IManager> imanager = new Mock<IManager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Founder = founder;
            store.Managers.TryAdd("manager", manager);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckValidAddProduct()
        {
            Assert.AreEqual(store.AddProduct(product1, "founder"), "Product added");
            Assert.IsTrue(store.Products.ContainsKey(product1.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidName()
        {
            Product product2 = new Product("", 10, 10, 10, "category");
            Assert.AreEqual(store.AddProduct(product2, "founder"), "Invalid product");
            Assert.IsFalse(store.Products.ContainsKey(product2.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidPrice()
        {
            Product product2 = new Product("2", 10, 10, -10, "category");
            Assert.AreEqual(store.AddProduct(product2, "founder"), "Invalid product");
            Assert.IsFalse(store.Products.ContainsKey(product2.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductNoPermission()
        {
            Assert.AreEqual(store.AddProduct(product1, "manager"), "No permission");
            Assert.IsFalse(store.Products.ContainsKey(product1.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc23")]
        public void CheckAddProductInvalidUser()
        {
            Assert.AreEqual(store.AddProduct(product1, "no one"), "Invalid user");
            Assert.IsFalse(store.Products.ContainsKey(product1.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckValidRemoveProduct()
        {
            store.Products.TryAdd(product1.Id, product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "founder"), "Product removed");
            Assert.IsFalse(store.Products.TryRemove(product1.Id, out _));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidUser()
        {;
            store.Products.TryAdd(product1.Id, product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "no one"), "Invalid user");
            Assert.IsTrue(store.Products.TryRemove(product1.Id, out _));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        [TestCategory("uc24")]
        public void CheckRemoveProductInvalidPermission()
        {
            store.Products.TryAdd(product1.Id, product1);
            Assert.AreEqual(store.RemoveProduct(product1.Id, "manager"), "No Permission");
            Assert.IsTrue(store.Products.TryRemove(product1.Id, out _));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckValidEditProduct()
        {
            Product product2 = new Product("1", 10, 10, 20, "category");
            store.Products.TryAdd(product1.Id, product1);
            Assert.AreEqual(store.EditProduct(product1.Id, product2, "founder"), "Product edited");
            Assert.IsTrue(store.Products.TryRemove(product1.Id, out Product p));
            Assert.AreEqual(p.Price, 20);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditUnavailablwProduct()
        {
            Assert.AreEqual(store.EditProduct(product1.Id, product1, "founder"), "Product not in the store");
            Assert.IsFalse(store.Products.ContainsKey(product1.Id));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        [TestCategory("uc25")]
        public void CheckEditNoPermission()
        {
            Product product2 = new Product("1", 10, 10, 20, "category");
            store.Products.TryAdd(product1.Id, product1);
            Assert.AreEqual(store.EditProduct(product1.Id, product2, "manager"), "No Permission");
            Assert.IsTrue(store.Products.TryRemove(product1.Id, out Product p));
            Assert.AreEqual(p.Price, 10);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
