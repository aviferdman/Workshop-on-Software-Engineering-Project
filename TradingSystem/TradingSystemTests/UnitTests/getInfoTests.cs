using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.UnitTests
{
    class getInfoTests
    {
        static Address address;
        static BankAccount bankAccount;
        Store store;
        Founder founder;

        public getInfoTests()
        {
            address = new Address("1", "1", "1", "1");
            bankAccount = new BankAccount(1000, 1000);
            store = new Store("testStore", bankAccount, address);
            ICollection<IHistory> histories = new List<IHistory>();
            MemberState ms = new MemberState("founder", histories);
            founder = Founder.makeFounder(ms, store);
            store.Founder = founder;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckValidGetInfo()
        {
            Assert.AreEqual(store.getInfo("founder"), "Founder - founder\n");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoInvalidUser()
        {
            Assert.AreEqual(store.getInfo("no one"), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoNoPermission()
        {
            IManager manager;
            Mock<IManager> imanager = new Mock<IManager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Managers.TryAdd("manager", manager);
            Assert.AreEqual(store.getInfo("manager"), "No permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckValidGetInfoSpecific()
        {
            Assert.AreEqual(store.getInfoSpecific("founder", "founder"), "Founder - founder\n");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoSpecificInvalidUser()
        {
            Assert.AreEqual(store.getInfoSpecific("no one", "founder"), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoSpecificNoPermission()
        {
            IManager manager;
            Mock<IManager> imanager = new Mock<IManager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Managers.TryAdd("manager", manager);
            Assert.AreEqual(store.getInfoSpecific("manager", "founder"), "No permission");
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
