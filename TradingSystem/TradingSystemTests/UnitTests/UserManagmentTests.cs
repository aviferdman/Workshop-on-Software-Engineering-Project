using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using System;

namespace TradingSystemTests
{
    [TestClass]
    public class UserManagmentTests
    {
        
        private string signup()
        {
           return UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala","lala","la"), "0501234733", false);
        }

        private bool delete()
        {
            return UserManagement.Instance.DeleteUser("inbi2001");
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc1")]
        public void TestSignUpSuccess()
        {
            Assert.AreEqual("success", signup());
            delete();
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc1")]
        public void TestSignUpFail()
        {
            signup();
            Assert.AreNotEqual("success", signup());
            Assert.IsTrue(UserManagement.Instance.DataUsers.ContainsKey("inbi2001"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginSuccess()
        {
            signup();
            DataUser d;
            Assert.AreEqual("success", UserManagement.Instance.LogIn("inbi2001", "123456"));
            UserManagement.Instance.DataUsers.TryGetValue("inbi2001", out d);
            Assert.IsTrue(d.IsLoggedin);
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed1()
        {
            signup();
            UserManagement.Instance.LogIn("inbi2001", "123456");
            Assert.AreEqual("user is already logged in", UserManagement.Instance.LogIn("inbi2001", "123456"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed2()
        {
            signup();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            DataUser d;
            UserManagement.Instance.DataUsers.TryGetValue("inbi2001", out d);
            Assert.IsFalse(d.IsLoggedin);
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutSuccess()
        {
            signup();
            UserManagement.Instance.LogIn("inbi2001", "123456");
            Assert.AreNotEqual(null, UserManagement.Instance.Logout("inbi2001"));
            DataUser d;
            UserManagement.Instance.DataUsers.TryGetValue("inbi2001", out d);
            Assert.IsFalse(d.IsLoggedin);
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutFail1()
        {
            signup();
            Assert.IsFalse(UserManagement.Instance.Logout("inbi2001"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutFail2()
        {
            Assert.IsFalse(UserManagement.Instance.Logout("inbi200151"));

        }

    }
}
