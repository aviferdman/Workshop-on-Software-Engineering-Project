﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class HistoryTests
    {

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(string)"/>
        [TestMethod]
        public void GetUserHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            ICollection<IHistory> userHistory = user.GetUserHistory(user.Username);
            Assert.AreEqual(1, userHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(string)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetUserHistoryWithoutPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            user.GetUserHistory(user.Username);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Store)"/>
        [TestMethod]
        public void GetStoreHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            MarketUsers market = MarketUsers.Instance;
            MarketStores marketStores = MarketStores.Instance;
            //market.DeleteAllTests();
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = marketStores.CreateStore("storeTest", user.Username, bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            ICollection<IHistory> storeHistory = store.GetStoreHistory(user.Username);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(1, storeHistory.Count);

        }


        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Store)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetStoreHistoryWithoutPermission()
        {

            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            MarketUsers market = MarketUsers.Instance;
            MarketStores marketStores = MarketStores.Instance;
            //market.DeleteAllTests();
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = marketStores.CreateStore("storeTest", user.Username, bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            store.GetStoreHistory("false Username");

        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        public void GetAllHistoryWithPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState adminState = new AdministratorState(user.Username, user.UserHistory);
            user.ChangeState(adminState);
            Store store = new Store("storeTest", bankAccount, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Assert.IsTrue(user.PurchaseShoppingCart(bankAccount, "0544444444", address));
            ICollection<IHistory> allHistory = user.State.GetAllHistory();
            Assert.IsNotNull(allHistory);
            Assert.AreEqual(2, allHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetAllHistoryWithoutPermission()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
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
