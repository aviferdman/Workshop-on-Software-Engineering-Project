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

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class AssignAndPermissionTests
    {
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;

        [TestInitialize]
        public async Task Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = false;
           
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager", "123", null);
            await marketUsers.AddMember("manager", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("owner", "123", null);
            await marketUsers.AddMember("owner", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "founder", card, address);
            await market.makeManager("manager", store.Id, "founder");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckValidMakeOwner()
        {
            String res = await market.makeOwner("owner", store.Id, "founder");
            Assert.AreEqual(res, "Success");
            Assert.IsTrue(store.Contains("owner", "Owners"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckMakeOwnerAlreadyAssigned()
        {
            await market.makeOwner("owner", store.Id, "founder");
            String res = await market.makeOwner("owner", store.Id, "founder");
            Assert.AreEqual(res, "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckMakeOwnerInvalidAssigner()
        {
            await market.makeManager("manager", store.Id, "founder");
            String res = await market.makeOwner("owner", store.Id, "manager");
            Assert.AreEqual(res, "Invalid assigner");
            Assert.IsFalse(store.Contains("owner", "Owners"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckMakeOwnerNotMember()
        {
            String res = await market.makeOwner("no one", store.Id, "founder");
            Assert.AreEqual(res, "the assignee isn't a member");
            Assert.IsFalse(store.Contains("owner", "Owners"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public async Task CheckValidMakeManager()
        {
            String guestName2 = marketUsers.AddGuest();
            await userManagement.SignUp("manager2", "123", null);
            await marketUsers.AddMember("manager2", "123", guestName2);
            String res = await market.makeManager("manager2", store.Id, "founder");
            Assert.AreEqual(res, "Success");
            Assert.IsTrue(store.Contains("manager2", "Managers"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public async Task CheckMakeManagerAlreadyAssigned()
        {
            await market.makeManager("manager", store.Id, "founder");
            String res = await market.makeManager("manager", store.Id, "founder");
            Assert.AreEqual(res, "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public async Task CheckMakeManagerInvalidAssigner()
        {
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager2", "123", null);
            await marketUsers.AddMember("manager2", "123", guestName);
            String res = await market.makeManager("manager2", store.Id, "manager");
            Assert.AreEqual(res, "Invalid assigner");
            Assert.IsFalse(store.Contains("manager2", "Managers"));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckValidDefinePermissions()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            String res = await market.DefineManagerPermissions("manager", store.Id, "founder", permissions);
            Assert.AreEqual(res, "Success");
            Manager manager = store.GetManager("manager");
            Assert.IsTrue(manager.GetPermission(Permission.AddProduct));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckDefinePermissionsInvalidAssigner()
        {
            await market.makeOwner("owner", store.Id, "founder");
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            String res = await market.DefineManagerPermissions("manager", store.Id, "owner", permissions);
            Assert.AreEqual(res, "Invalid assigner");
            Manager manager = store.GetManager("manager");
            Assert.IsFalse(manager.GetPermission(Permission.AddProduct));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.DefineManagerPermissions(Guid, Guid, List{Permission})"/>
        [TestMethod]
        [TestCategory("uc34")]
        public async Task CheckDefinePermissionsNoManager()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            String res = await market.DefineManagerPermissions("manager2", store.Id, "founder", permissions);
            Assert.AreEqual(res, "Manager doesn't exist");
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
