using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
/*
namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class HistoryTests
    {

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        public void GetUserHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            MemberState memberState = new MemberState(user.Id);
            user.ChangeState(memberState);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            History userHistory = user.GetUserHistory(user.Id);
            Assert.IsNotNull(userHistory);
            Assert.AreEqual(1, userHistory.Deliveries.Count);
            Assert.AreEqual(1, userHistory.Deliveries.Count);
            Assert.AreEqual(1, userHistory.Products.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetUserHistoryWithoutPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            user.GetUserHistory(user.Id);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Guid)(Guid)"/>
        [TestMethod]
        public void GetStoreHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Id);
            user.ChangeState(memberState);
            Market market = Market.Instance;
            market.DeleteAllTests();
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = market.CreateStore("storeTest", user.Username, bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            History storeHistory = store.GetStoreHistory(user.Id);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(1, storeHistory.Deliveries.Count);
            Assert.AreEqual(1, storeHistory.Deliveries.Count);
            Assert.AreEqual(1, storeHistory.Products.Count);

        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Guid)(Guid)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetStoreHistoryWithoutPermission()
        {
            
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Market market = Market.Instance;
            market.ActiveUsers.TryAdd(user.Username, user);
            market.Stores.TryAdd(store.Id, store);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            market.GetStoreHistory(user.Username, store.Id);

        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        public void GetAllHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState adminState = new AdministratorState(user.Id);
            user.ChangeState(adminState);
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            History allHistory = user.State.GetAllHistory();
            Assert.IsNotNull(allHistory);
            Assert.AreEqual(1, allHistory.Deliveries.Count);
            Assert.AreEqual(1, allHistory.Deliveries.Count);
            Assert.AreEqual(1, allHistory.Products.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetAllHistoryWithoutPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            user.State.GetAllHistory();

        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
*/