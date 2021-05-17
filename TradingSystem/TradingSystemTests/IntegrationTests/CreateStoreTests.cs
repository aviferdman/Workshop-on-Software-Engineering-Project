using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class CreateStoreTests
    {
        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.CreateStore(string, string, CreditCard, Address)"/>
        [TestMethod]
        public void CreateStoreAndCheckFounder()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            Store store = market.CreateStore("storeTest", user.Username, card, address);
            Assert.IsNotNull(store);
            Assert.AreEqual(store.Founder.Username, user.Username);

        }

        /// test for function :<see cref="TradingSystem.Business.Market..MarketStores.CreateStore(string, string, BankAccount, Address)"/>
        [TestMethod]
        public void NotCreateStoreAndCheckNotFounder()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            User user2 = new User("testUser2");
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            marketUsers.ActiveUsers.TryAdd(user2.Username, user2);
            Store store = market.CreateStore("storeTest", user.Username, card, address);
            Assert.IsNotNull(store);
            Assert.AreNotEqual(store.Founder.Username, user2.Username);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
