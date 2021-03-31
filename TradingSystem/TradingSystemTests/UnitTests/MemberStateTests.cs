using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class MemberStateTests
    {

        //START UNIT TESTING

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        public void GetUserHistoryWithPermission()
        {
            Guid id = Guid.NewGuid();
            Mock<IStorePermission> storePermission = new Mock<IStorePermission>();
            storePermission.Setup(sp => sp.GetUserHistory(It.IsAny<Guid>())).Returns(true);

            MemberState memberState = new MemberState(id, storePermission.Object);
            Assert.IsNotNull(memberState.GetUserHistory(id));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetUserHistoryWithoutPermission()
        {
            Guid id = Guid.NewGuid();
            Mock<IStorePermission> storePermission = new Mock<IStorePermission>();
            storePermission.Setup(sp => sp.GetUserHistory(It.IsAny<Guid>())).Returns(false);

            MemberState memberState = new MemberState(id, storePermission.Object);
            memberState.GetUserHistory(id);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Guid)"/>
        [TestMethod]
        public void GetStoreHistoryWithPermission()
        {
            Guid id = Guid.NewGuid();
            Mock<IStorePermission> storePermission = new Mock<IStorePermission>();
            storePermission.Setup(sp => sp.GetStoreHistory(It.IsAny<Guid>())).Returns(true);

            MemberState memberState = new MemberState(id, storePermission.Object);
            Assert.IsNotNull(memberState.GetStoreHistory(id));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Guid)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetStoreHistoryWithoutPermission()
        {
            Guid id = Guid.NewGuid();
            Mock<IStorePermission> storePermission = new Mock<IStorePermission>();
            storePermission.Setup(sp => sp.GetStoreHistory(It.IsAny<Guid>())).Returns(false);

            MemberState memberState = new MemberState(id, storePermission.Object);
            memberState.GetStoreHistory(id);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

        //END UNIT TESTING
    }
}
