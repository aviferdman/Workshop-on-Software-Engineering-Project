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
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;
namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class RemoveOwnerTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;
        Founder founder;
        Owner owner1;
        Owner owner2;
        Owner owner3;

        [TestInitialize]
        public async Task Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null, null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("owner1", "123", null, null);
            await marketUsers.AddMember("owner1", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("owner2", "123", null, null);
            await marketUsers.AddMember("owner2", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("owner3", "123", null, null);
            await marketUsers.AddMember("owner3", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "founder", card, address);
            await market.makeOwner("owner1", store.Id, "founder");
            await market.makeOwner("owner2", store.Id, "owner1");
            await market.makeOwner("owner3", store.Id, "owner1");
            founder = store.Founder;
            owner1 = store.GetOwner("owner1");
            owner2 = store.GetOwner("owner2"); 
            owner3 = store.GetOwner("owner3"); 
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task checkValidRemoveOwner()
        {
            String res = await market.RemoveOwner("owner2", store.Id, "owner1");
            Assert.AreEqual(res, "success");
            Assert.IsFalse(store.Contains("owner2", "Owners"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task checkValidRemoveOwnerChain()
        {
            String res = await market.RemoveOwner("owner1", store.Id, "founder");
            Assert.AreEqual(res, "success");
            Assert.IsFalse(store.Contains("owner1", "Owners"));
            Assert.IsFalse(store.Contains("owner2", "Owners"));
            Assert.IsFalse(store.Contains("owner3", "Owners"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task checkRemoveOwnerDoesntExist()
        {
            String res = await market.RemoveOwner("no one", store.Id, "owner1");
            Assert.AreEqual(res, "Owner doesn't exist");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task checkRemoveOwnerInvalidAssigner()
        {
            String res = await market.RemoveOwner("owner2", store.Id, "founder");
            Assert.AreEqual(res, "Invalid Assigner");
            Assert.IsTrue(store.Contains("owner2", "Owners"));
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
