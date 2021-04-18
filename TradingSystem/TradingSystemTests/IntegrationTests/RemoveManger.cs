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
    class RemoveManger
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;
        Founder founder;
        Owner owner;
        Manager manager;

        [TestInitialize]
        public void Initialize()
        {
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("founder", "123", null, null);
            marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("manager", "123", null, null);
            marketUsers.AddMember("manager", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("owner", "123", null, null);
            marketUsers.AddMember("owner", "123", guestName);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            store = market.CreateStore("testStore", "founder", bankAccount, address);
            market.makeManager("manager", store.Id, "founder");
            market.makeOwner("owner", store.Id, "founder");
            founder = store.Founder;
            store.Owners.TryGetValue("owner", out owner);
            store.Managers.TryGetValue("manager", out IManager imanager);
            manager = (Manager)imanager;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkValidRemoveManager()
        {
            Assert.AreEqual(market.RemoveManager("manager", store.Id, "founder"), "success");
            Assert.IsFalse(store.Managers.ContainsKey("manager"));
            Assert.IsFalse(founder.ManagerAppointments.ContainsKey("manager"));
            Assert.IsFalse(manager.M.ManagerPrems.ContainsKey(store));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkRemoveManagerWrongStore()
        {
            Assert.AreEqual(market.RemoveManager("manager2", store.Id, "founder"), "Manager doesn't exist");
            Assert.IsTrue(store.Managers.ContainsKey("manager"));
            Assert.IsTrue(founder.ManagerAppointments.ContainsKey("manager"));
            Assert.IsTrue(manager.M.ManagerPrems.ContainsKey(store));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.RemoveManager(string, Guid, string)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void checkRemoveManagerBadAppointer()
        {
            Assert.AreEqual(market.RemoveManager("manager", store.Id, "owner"), "Invalid Assigner");
            Assert.IsTrue(store.Managers.ContainsKey("manager"));
            Assert.IsTrue(founder.ManagerAppointments.ContainsKey("manager"));
            Assert.IsTrue(manager.M.ManagerPrems.ContainsKey(store));
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
