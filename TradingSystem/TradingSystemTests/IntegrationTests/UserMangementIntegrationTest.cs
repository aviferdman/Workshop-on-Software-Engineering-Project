using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.UserManagement;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class UserMangementIntegrationTest
    {
        private String signup()
        {
            UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733");
            return MarketUsers.Instance.AddGuest();
        }

        private bool delete(string username)
        {
            MemberState m;
            MarketUsers.Instance.logout(username);
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.MemberStates.TryRemove(username, out m);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }
        private bool delete2(string username)
        {
            MemberState m;
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.MemberStates.TryRemove(username, out m);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }


        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginSuccess()
        {
            string user=signup();
            User u;
            Assert.AreEqual("success", MarketUsers.Instance.AddMember("inbi2001", "123456", user));
            Assert.IsTrue(MarketUsers.Instance.ActiveUsers.TryGetValue("inbi2001", out u));
            Assert.IsInstanceOfType(u.State, typeof(MemberState));
            delete("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed1()
        {
            string user = signup();
            MarketUsers.Instance.AddMember("inbi2001", "123456", user);
            Assert.AreEqual("user is already logged in", MarketUsers.Instance.AddMember("inbi2001", "123456", user));
            delete("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed2()
        {
            string user = signup();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", MarketUsers.Instance.AddMember("inbi2001", "12345d6", user));
            Assert.IsFalse(MarketUsers.Instance.ActiveUsers.ContainsKey("inbi2001"));
            delete("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", MarketUsers.Instance.AddMember("inbi2001", "12345d6","lilk"));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutSuccess()
        {
            string user = signup();
            MarketUsers.Instance.AddMember("inbi2001", "123456", user);
            Assert.AreNotEqual(null, MarketUsers.Instance.logout("inbi2001"));
            Assert.IsFalse(MarketUsers.Instance.ActiveUsers.ContainsKey("inbi2001"));
            delete(user);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutFail1()
        {
            string user = signup();
            Assert.AreEqual(null, MarketUsers.Instance.logout("inbi2001"));
            delete2(user);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutFail2()
        {
            Assert.AreEqual(null, MarketUsers.Instance.logout("inbi200151"));
        }

    }
}
