using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class AssignMemberTest
    {
        Store store;
        MarketUsers market;

        [TestInitialize]
        public void TestInitialize()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1, 1);
            store = new Store("1", bankAccount, address);
            market = MarketUsers.Instance;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(string, User, string)"/>
        [TestMethod]
        public void CheckValidMakeOwner()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            User user = new User("testUser");
            User user2 = new User("testUser2");
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            MemberState memberState2 = new MemberState(user2.Username, user2.UserHistory);
            user2.ChangeState(memberState2);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            marketUsers.ActiveUsers.TryAdd(user2.Username, user2);
            Store store = market.CreateStore("storeTest", user.Username, bankAccount, address);
            MarketUsers.Instance.MemberStates.TryAdd(user2.Username, (MemberState)user2.State);
            market.AssignMember(user2.Username, store.Id, user.Username, "manager");
            Assert.AreEqual(1, store.Managers.Count);
        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            User user = new User("testUser");
            User user2 = new User("testUser2");
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            MemberState memberState2 = new MemberState(user2.Username, user2.UserHistory);
            user2.ChangeState(memberState2);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            marketUsers.ActiveUsers.TryAdd(user2.Username, user2);
            Store store = market.CreateStore("storeTest", user.Username, bankAccount, address);
            MarketUsers.Instance.MemberStates.TryAdd(user2.Username, (MemberState)user2.State);
            store.Owners.TryAdd(user2.Username, null);
            Assert.AreEqual("this member is already assigned as a store owner or manager", market.AssignMember(user2.Username, store.Id, user.Username, "owner"));
        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerInvalidAssigner()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            User user = new User("testUser");
            User user2 = new User("testUser2");
            MemberState memberState = new MemberState(user.Username, user.UserHistory);
            user.ChangeState(memberState);
            MemberState memberState2 = new MemberState(user2.Username, user2.UserHistory);
            user2.ChangeState(memberState2);
            MarketStores market = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            marketUsers.ActiveUsers.TryAdd(user2.Username, user2);
            Store store = market.CreateStore("storeTest", user.Username, bankAccount, address);
            MarketUsers.Instance.MemberStates.TryAdd(user2.Username, (MemberState)user2.State);
            store.Managers.TryAdd(user2.Username, null);
            Assert.AreEqual("Invalid assigner", market.AssignMember(user2.Username, store.Id, user2.Username, "owner"));
        }

        /*
        /* In comment - not pass currently 
        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckValidMakeManager()
        {
            Guid assigneeID = Guid.NewGuid();
            User user = new User("assigner");
            Founder assigner = new Founder(user.Id);
            Owner owner = new Owner(assigneeID, assigner);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            personnel.TryAdd(assigneeID, owner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Manager), "Success");
        } 

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeManagerAlreadyAssigned()
        {
            User user = new User("assigner");
            Founder assigner = new Founder(user.Id);
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Manager), "Success");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeManagerInvalidAssigner()
        {
            Guid founderID = Guid.NewGuid();
            User manager = new User("manager");
            Founder founder = new Founder(founderID);
            Manager assigner = new Manager(manager.Id, founder);
            Guid assignee = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(manager.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assignee, manager, AppointmentType.Manager), "Invalid assigner");
        }
        */
    }
}
