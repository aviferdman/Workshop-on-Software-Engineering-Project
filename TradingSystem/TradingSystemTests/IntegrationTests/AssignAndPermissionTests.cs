﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class AssignAndPermissionTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;

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
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = market.CreateStore("testStore", "founder", card, address);
            market.makeManager("manager", store.Id, "founder");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckValidMakeOwner()
        {
            Assert.AreEqual(market.makeOwner("owner", store.Id, "founder"), "Success");
            Assert.IsTrue(store.Owners.ContainsKey("owner"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            market.makeOwner("owner", store.Id, "founder");
            Assert.AreEqual(market.makeOwner("owner", store.Id, "founder"), "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckMakeOwnerInvalidAssigner()
        {
            market.makeManager("manager", store.Id, "founder");
            Assert.AreEqual(market.makeOwner("owner", store.Id, "manager"), "Invalid assigner");
            Assert.IsFalse(store.Owners.ContainsKey("owner"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckMakeOwnerNotMember()
        {
            Assert.AreEqual(market.makeOwner("no one", store.Id, "founder"), "the assignee isn't a member");
            Assert.IsFalse(store.Owners.ContainsKey("owner"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public void CheckValidMakeManager()
        {
            String guestName2 = marketUsers.AddGuest();
            userManagement.SignUp("manager2", "123", null, null);
            marketUsers.AddMember("manager2", "123", guestName2);
            Assert.AreEqual(market.makeManager("manager2", store.Id, "founder"), "Success");
            Assert.IsTrue(store.Managers.ContainsKey("manager2"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public void CheckMakeManagerAlreadyAssigned()
        {
            market.makeManager("manager", store.Id, "founder");
            Assert.AreEqual(market.makeManager("manager", store.Id, "founder"), "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public void CheckMakeManagerInvalidAssigner()
        {
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("manager2", "123", null, null);
            marketUsers.AddMember("manager2", "123", guestName);
            Assert.AreEqual(market.makeManager("manager2", store.Id, "manager"), "Invalid assigner");
            Assert.IsFalse(store.Managers.ContainsKey("manager2"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckValidDefinePermissions()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.AreEqual(market.DefineManagerPermissions("manager", store.Id, "founder", permissions), "Success");
            store.Managers.TryGetValue("manager", out IManager manager);
            Assert.IsTrue(manager.GetPermission(Permission.AddProduct));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckDefinePermissionsInvalidAssigner()
        {
            market.makeOwner("owner", store.Id, "founder");
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.AreEqual(market.DefineManagerPermissions("manager", store.Id, "owner", permissions), "Invalid assigner");
            store.Managers.TryGetValue("manager", out IManager manager);
            Assert.IsFalse(manager.GetPermission(Permission.AddProduct));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public void CheckDefinePermissionsNoManager()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.AreEqual(market.DefineManagerPermissions("manager2", store.Id, "founder", permissions), "Manager doesn't exist");
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
