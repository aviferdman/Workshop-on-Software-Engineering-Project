using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.UnitTests
{
    class getInfoTests
    {
        static Address address;
        static CreditCard card;
        Store store;
        Founder founder;

        public getInfoTests()
        {
            address = new Address("1", "1", "1", "1", "1");
            card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = new Store("testStore", card, address);
            MemberState ms = new MemberState("founder");
            founder = Founder.makeFounder(ms, store);
            store.Founder = founder;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckValidGetInfo()
        {
            Assert.AreEqual(store.GetInfo("founder"), new WorkerDetails(founder));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoInvalidUser()
        {
            Assert.AreEqual(store.GetInfo("no one"), null);
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
            Assert.AreEqual(store.GetInfo("manager"), null);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckValidGetInfoSpecific()
        {
            Assert.AreEqual(store.GetInfoSpecific("founder", "founder"), new WorkerDetails(founder));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoSpecificInvalidUser()
        {
            Assert.AreEqual(store.GetInfoSpecific("no one", "founder"), null);
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
            Assert.AreEqual(store.GetInfoSpecific("manager", "founder"), null);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
