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
        private string signup()
        {
            UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733");
            return MarketUsers.Instance.AddGuest();
        }

        private bool delete(string username)
        {
            MarketUsers.Instance.logout(username);
            MarketUsers.Instance.RemoveGuest(username);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }
        private bool delete2(string username)
        {
            MarketUsers.Instance.RemoveGuest(username);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }


        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginSuccess()
        {
            string user=signup();
            
            Assert.AreEqual("success", UserManagement.Instance.LogIn("inbi2001", "123456", "lala"));
            delete(user);

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed1()
        {
            string user = signup();
            UserManagement.Instance.LogIn("inbi2001", "123456", "lala");
            Assert.AreEqual("user is already logged in", UserManagement.Instance.LogIn("inbi2001", "123456", "lala"));
            delete(user);

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed2()
        {
            string user = signup();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", UserManagement.Instance.LogIn("inbi2001", "12345d6", "lala"));
            delete(user);

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", UserManagement.Instance.LogIn("inbi2001", "12345d6", "lala"));

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutSuccess()
        {
            string user = signup();
            UserManagement.Instance.LogIn("inbi2001", "123456", "lala");
            Assert.AreNotEqual(null, UserManagement.Instance.Logout("inbi2001"));
            delete(user);

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutFail1()
        {
            string user = signup();
            Assert.AreEqual(null, UserManagement.Instance.Logout("inbi2001"));
            delete2(user);

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutFail2()
        {
            Assert.AreEqual(null, UserManagement.Instance.Logout("inbi200151"));

        }

    }
}
