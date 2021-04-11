using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Service;
/*
namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class CreateStoreTests
    {
        /// test for function :<see cref="TradingSystem.Business.Market.User.CreateStore(string, TradingSystem.Business.Market.BankAccount, TradingSystem.Business.Market.Address)"/>
        [TestMethod]
        public void CreateStoreAndCheckFounder()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Id);
            user.ChangeState(memberState);
            Market market = Market.Instance;
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = market.CreateStore("storeTest", user.Username, bankAccount, address);
            StorePermission storePermission = store.Personnel[user.Id];
            Assert.IsNotNull(storePermission);
            Type storePermissionType = storePermission.GetType();
            Assert.AreEqual(storePermissionType, new Founder(user.Id).GetType());

        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.User.CreateStore(string, TradingSystem.Business.Market.BankAccount, TradingSystem.Business.Market.Address)"/>
        [TestMethod]
        public void NotCreateStoreAndCheckNotFounder()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            User founder = new User("testUser");
            User owner = new User("testUser");
            MemberState memberState = new MemberState(founder.Id);
            founder.ChangeState(memberState);
            Market market = Market.Instance;
            market.DeleteAllTests();
            market.ActiveUsers.TryAdd(founder.Username, founder);
            market.ActiveUsers.TryAdd(owner.Username, owner);
            Store store = market.CreateStore("storeTest", founder.Username, bankAccount, address);
            store.AddPerssonel(founder.Id, owner.Id, AppointmentType.Owner);
            StorePermission founderStorePermission = store.Personnel[founder.Id];
            StorePermission ownerStorePermission = store.Personnel[owner.Id];
            Assert.IsNotNull(founderStorePermission);
            Assert.IsNotNull(ownerStorePermission);
            Type storePermissionType1 = founderStorePermission.GetType();
            Type storePermissionType2 = ownerStorePermission.GetType();
            Assert.AreEqual(storePermissionType1, new Founder(founder.Id).GetType());
            Assert.AreEqual(storePermissionType2, new Owner(founder.Id, founderStorePermission).GetType());
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}*/
