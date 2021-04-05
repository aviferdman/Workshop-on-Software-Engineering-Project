using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class CreateStoreTests
    {
        /// test for function :<see cref="TradingSystem.Business.Market.User.CreateStore(string, TradingSystem.Business.Market.BankAccount, TradingSystem.Business.Market.Address)"/>
 /*       [TestMethod]
        public void CreateStoreAndCheckFounder()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Id, user.StorePermission);
            user.ChangeState(memberState);
            Store store = user.CreateStore("storeTest", bankAccount, address);
            int userHierarchy = user.StorePermission.GetHierarchy(store.Id);
            Permission userPermission = user.StorePermission.GetPermission(store.Id);
            Assert.AreEqual(1, userHierarchy);
            Assert.AreEqual(Permission.Founder, userPermission);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.CreateStore(string, TradingSystem.Business.Market.BankAccount, TradingSystem.Business.Market.Address)"/>
        [TestMethod]
        public void NotCreateStoreAndCheckNotFounder()
        {
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Address address = new Address("1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            User user2 = new User("testUser2");
            MemberState memberState = new MemberState(user.Id, user.StorePermission);
            user.ChangeState(memberState);
            Store store = user.CreateStore("storeTest", bankAccount, address);
            int user2Hierarchy = user2.StorePermission.GetHierarchy(store.Id);
            Permission user2Permission = user2.StorePermission.GetPermission(store.Id);
            Assert.AreNotEqual(1, user2Hierarchy);
            Assert.AreNotEqual(Permission.Founder, user2Permission);

        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }*/
    }
}
