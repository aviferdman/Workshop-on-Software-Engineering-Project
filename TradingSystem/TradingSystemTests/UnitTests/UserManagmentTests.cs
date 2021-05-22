using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using System;
using TradingSystem.DAL;

namespace TradingSystemTests
{
    [TestClass]
    public class UserManagmentTests
    {
        public UserManagmentTests()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }
        
        private async System.Threading.Tasks.Task<string> signupAsync()
        {
           return await UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala","lala","la", "1111111"), "0501234733");
        }

        private async System.Threading.Tasks.Task<bool> deleteAsync()
        {
            return await UserManagement.Instance.DeleteUser("inbi2001");
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc1")]
        public async void TestSignUpSuccess()
        {
            Assert.AreEqual("success", signupAsync());
            await deleteAsync();
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc1")]
        public async void TestSignUpFail()
        {
            await signupAsync();
            Assert.AreNotEqual("success", signupAsync());
            Assert.IsNotNull(await ProxyMarketContext.Instance.GetDataUser("inbi2001"));
            await deleteAsync();
            
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestLoginSuccess()
        {
            await signupAsync();
            DataUser d;
            Assert.AreEqual("success", UserManagement.Instance.LogIn("inbi2001", "123456"));
            d=await ProxyMarketContext.Instance.GetDataUser("inbi2001");
            Assert.IsTrue(d.isLoggedin);
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestLoginFailed1()
        {
            await signupAsync();
            await UserManagement.Instance.LogIn("inbi2001", "123456");
            Assert.AreEqual("user is already logged in", UserManagement.Instance.LogIn("inbi2001", "123456"));
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public async System.Threading.Tasks.Task TestLoginFailed2Async()
        {
            await signupAsync();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            DataUser d;
            d = await ProxyMarketContext.Instance.GetDataUser("inbi2001");
            Assert.IsFalse(d.isLoggedin);
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public async void TestLogoutSuccess()
        {
            await signupAsync();
            await UserManagement.Instance.LogIn("inbi2001", "123456");
            Assert.AreNotEqual(null, UserManagement.Instance.Logout("inbi2001"));
            DataUser d;
            d = await ProxyMarketContext.Instance.GetDataUser("inbi2001");
            Assert.IsFalse(d.isLoggedin);
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public async void TestLogoutFail1()
        {
            await signupAsync();
            Assert.IsFalse(await UserManagement.Instance.Logout("inbi2001"));
            await deleteAsync();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.Logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public async void TestLogoutFail2()
        {
            Assert.IsFalse(await UserManagement.Instance.Logout("inbi200151"));

        }

    }
}
