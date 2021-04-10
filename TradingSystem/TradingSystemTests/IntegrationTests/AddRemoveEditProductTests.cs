using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;


namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class AddRemoveEditProductTests
    {
        Store store;
        Market market;

        [TestInitialize]
        public void TestInitialize()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount( 1, 1);
            store = new Store("1", bankAccount, address);
            market = Market.Instance;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckValidAddProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Product added");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductUnauthorizedUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            Guid user = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidPrice()
        {
            Product product1 = new Product("1", 10, 10, -10);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidName()
        {
            Product product1 = new Product("", 10, 10, 10);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, founderID), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckValidRemoveProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", founderID), "Product removed");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Guid userID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", userID), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Guid founderID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            Manager manager = new Manager(managerID, founder);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", managerID), "No Permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckValidEditProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, founderID), "Product edited");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        public void CheckEditUnavailablwProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Guid founderID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("2", product2, founderID), "Product not in the store");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckEditNoPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Guid founderID = Guid.NewGuid();
            Guid managerID = Guid.NewGuid();
            Founder founder = new Founder(founderID);
            Manager manager = new Manager(managerID, founder);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(founderID, founder);
            personnel.TryAdd(managerID, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, managerID), "No Permission");
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
