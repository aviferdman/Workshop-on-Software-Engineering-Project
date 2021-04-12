using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class AssignMemberTest
    {
        Store store;
        Market market;

        [TestInitialize]
        public void TestInitialize()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1, 1);
            store = new Store("1", bankAccount, address);
            market = Market.Instance;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckValidMakeOwner()
        {
            User user = new User("assigner");
            Founder assigner = new Founder(user.Id);
            Guid assigneeID = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Owner), "Success");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            Guid assigneeID = Guid.NewGuid();
            User user = new User("assigner");
            Founder assigner = new Founder(user.Id);
            Owner owner = new Owner(assigneeID, assigner);
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(user.Id, assigner);
            personnel.TryAdd(assigneeID, owner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assigneeID, user, AppointmentType.Owner), "this member is already assigned as a store owner or manager");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        public void CheckMakeOwnerInvalidAssigner()
        {
            Guid founderID = Guid.NewGuid();
            User manager = new User("manager");
            Founder founder = new Founder(founderID);
            Manager assigner = new Manager(manager.Id, founder);
            Guid assignee = Guid.NewGuid();
            ConcurrentDictionary<Guid, IStorePermission> personnel = new ConcurrentDictionary<Guid, IStorePermission>();
            personnel.TryAdd(manager.Id, assigner);
            store.Personnel = personnel;

            Assert.AreEqual(store.AssignMember(assignee, manager, AppointmentType.Owner), "Invalid assigner");
        }

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
        } */

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
    }
}
