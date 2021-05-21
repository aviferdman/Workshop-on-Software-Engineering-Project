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
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class RemoveMangerTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;

        [TestInitialize]
        public async void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null, null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager", "123", null, null);
            await marketUsers.AddMember("manager", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("owner", "123", null, null);
            await marketUsers.AddMember("owner", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "founder", card, address);
            await market.makeManager("manager", store.Id, "founder");
            await market.makeOwner("owner", store.Id, "founder");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckValidRemoveManager()
        {
            Assert.AreEqual(market.RemoveManager("manager", store.Id, "founder"), "success");
            Assert.IsFalse(store.Contains("manager", "Managers"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckRemoveManagerWrongStore()
        {
            Assert.AreEqual(market.RemoveManager("manager2", store.Id, "founder"), "Manager doesn't exist");
            Assert.IsTrue(store.Contains("manager", "Managers"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckRemoveManagerBadAppointer()
        {
            Assert.AreEqual(market.RemoveManager("manager", store.Id, "owner"), "Invalid Assigner");
            Assert.IsTrue(store.Contains("manager", "Managers"));
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
