using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class MemberStateTests
    {

        //START UNIT TESTING

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        public void GetUserHistoryWithCorrectUserId()
        {
            User user = new User("UserTest");
            MemberState memberState = new MemberState(user.Id, new UserHistory());
            user.ChangeState(memberState);
            Assert.IsNotNull(memberState.GetUserHistory(user.Id)); // succeeded and returns an object

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(Guid)"/>
        [TestMethod]
        public void GetUserHistoryWithWrongUserId()
        {
            User user = new User("UserTest");
            MemberState memberState = new MemberState(user.Id, new UserHistory());
            user.ChangeState(memberState);
            Assert.IsNotNull(memberState.GetUserHistory(Guid.NewGuid())); // succeeded and returns an object

        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

        //END UNIT TESTING
    }
}
