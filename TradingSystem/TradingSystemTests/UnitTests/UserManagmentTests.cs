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
        [ClassInitialize]
        public static void setMarket(TestContext testContext)
        {
            Mock<IMarket> market = new Mock<IMarket>();
            market.Setup(m => m.AddMember(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
            market.Setup(m => m.logout(It.IsAny<string>())).Returns(true);
            UserManagement.Instance.Marketo = market.Object;
        }

        private string signup()
        {
           return UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala","lala","la"), "0501234733");
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
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginSuccess()
        {
            signup();
            Assert.AreEqual("success", UserManagement.Instance.LogIn("inbi2001", "123456","lala"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed1()
        {
            signup();
            UserManagement.Instance.LogIn("inbi2001", "123456", "lala");
            Assert.AreEqual("user is already logged in", UserManagement.Instance.LogIn("inbi2001", "123456", "lala"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed2()
        {
            signup();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", UserManagement.Instance.LogIn("inbi2001", "12345d6", "lala"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public void TestLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", UserManagement.Instance.LogIn("inbi2001", "12345d6", "lala"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutSuccess()
        {
            signup();
            UserManagement.Instance.LogIn("inbi2001", "123456", "lala");
            Assert.AreEqual(true, UserManagement.Instance.Logout("inbi2001"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutFail1()
        {
            signup();
            Assert.AreEqual(false, UserManagement.Instance.Logout("inbi2001"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public void TestLogoutFail2()
        {
            Assert.AreEqual(false, UserManagement.Instance.Logout("inbi200151"));

        }

        [ClassCleanup]
        public static void removeMockMarket() {
            UserManagement.Instance.Marketo = Market.Instance;
        }
    }
}
