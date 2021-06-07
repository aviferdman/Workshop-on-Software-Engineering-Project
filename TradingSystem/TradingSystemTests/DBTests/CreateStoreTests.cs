using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.DAL;
using TradingSystem.Service;

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class CreateStoreTests
    {
        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.CreateStore(string, string, CreditCard, Address)"/>
        [TestMethod]
        public async Task CreateStoreAndCheckFounder()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            Store store = await market.CreateStore("storeTest", user.Username, card, address);
            Assert.IsNotNull(store);
            Assert.AreEqual(store.Founder.Username, user.Username);

        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = false;
        }

        /// test for function :<see cref="TradingSystem.Business.Market..MarketStores.CreateStore(string, string, BankAccount, Address)"/>
        [TestMethod]
        public async Task NotCreateStoreAndCheckNotFounder()
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
            Store store = await market.CreateStore("storeTest", user.Username, card, address);
            Assert.IsNotNull(store);
            Assert.AreNotEqual(store.Founder.Username, user2.Username);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            MarketUsers.Instance.tearDown();
        }
    }
}
