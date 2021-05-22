using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    public class getInfoTests
    {
        static Address address;
        static CreditCard card;
        Store store;
        Founder founder;

        public getInfoTests()
        {
            ProxyMarketContext.Instance.IsDebug = true;
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
            List<WorkerDetails> workers = new List<WorkerDetails>();
            workers.Add(new WorkerDetails(founder));
            List<WorkerDetails> res = store.GetInfo("founder");
            Assert.AreEqual(res.Count, 1);
            Assert.IsTrue(res[0].Equals(workers[0]));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoInvalidUser()
        {
            Assert.AreEqual(store.GetInfo("no one").Count, 0);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfo(string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckGetInfoNoPermission()
        {
            Manager manager;
            Mock<Manager> imanager = new Mock<Manager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Managers.Add(manager);
            Assert.AreEqual(store.GetInfo("manager").Count, 0);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.getInfoSpecific(string, string)"/>
        [TestMethod]
        [TestCategory("uc36")]
        public void CheckValidGetInfoSpecific()
        {
            WorkerDetails worker = new WorkerDetails(founder);
            WorkerDetails res = store.GetInfoSpecific("founder", "founder");
            Assert.IsTrue(res.Equals(worker));
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
            Manager manager;
            Mock<Manager> imanager = new Mock<Manager>();
            imanager.Setup(m => m.GetPermission(It.IsAny<Permission>())).Returns(false);
            manager = imanager.Object;
            store.Managers.Add(manager);
            Assert.AreEqual(store.GetInfoSpecific("founder", "manager"), null);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            store = null;
        }
    }
}
