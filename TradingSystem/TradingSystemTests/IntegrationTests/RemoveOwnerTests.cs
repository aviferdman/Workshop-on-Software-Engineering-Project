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
        public void Initialize()
        {
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("founder", "123", null, null);
            marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("owner1", "123", null, null);
            marketUsers.AddMember("owner1", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("owner2", "123", null, null);
            marketUsers.AddMember("owner2", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("owner3", "123", null, null);
            marketUsers.AddMember("owner3", "123", guestName);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            store = market.CreateStore("testStore", "founder", bankAccount, address);
            market.makeOwner("owner1", store.Id, "founder");
            market.makeOwner("owner2", store.Id, "owner1");
            market.makeOwner("owner3", store.Id, "owner1");
            founder = store.Founder;
            store.Owners.TryGetValue("owner1", out owner1);
            store.Owners.TryGetValue("owner2", out owner2);
            store.Owners.TryGetValue("owner3", out owner3);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkValidRemoveOwner()
        {
            Assert.AreEqual(market.RemoveOwner("owner2", store.Id, "owner1"), "success");
            Assert.IsFalse(store.Owners.ContainsKey("owner2"));
            Assert.IsFalse(owner1.getM().OwnerAppointments.ContainsKey("owner2"));
            Assert.IsFalse(owner2.getM().OwnerPrems.ContainsKey(store));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkValidRemoveOwnerChain()
        {
            Assert.AreEqual(market.RemoveOwner("owner1", store.Id, "founder"), "success");
            Assert.IsFalse(store.Owners.ContainsKey("owner1"));
            Assert.IsFalse(store.Owners.ContainsKey("owner2"));
            Assert.IsFalse(store.Owners.ContainsKey("owner3"));
            Assert.IsFalse(founder.getM().OwnerAppointments.ContainsKey("owner1"));
            Assert.IsFalse(owner1.getM().OwnerAppointments.ContainsKey("owner2"));
            Assert.IsFalse(owner1.getM().OwnerAppointments.ContainsKey("owner3"));
            Assert.IsFalse(owner1.getM().OwnerPrems.ContainsKey(store));
            Assert.IsFalse(owner2.getM().OwnerPrems.ContainsKey(store));
            Assert.IsFalse(owner3.getM().OwnerPrems.ContainsKey(store));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkRemoveOwnerDoesntExist()
        {
            Assert.AreEqual(market.RemoveOwner("no one", store.Id, "owner1"), "Owner doesn't exist");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkRemoveOwnerInvalidAssigner()
        {
            Assert.AreEqual(market.RemoveOwner("owner2", store.Id, "founder"), "Invalid Assigner");
            Assert.IsTrue(store.Owners.ContainsKey("owner2"));
            Assert.IsTrue(owner1.getM().OwnerAppointments.ContainsKey("owner2"));
            Assert.IsTrue(owner2.getM().OwnerPrems.ContainsKey(store));
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
