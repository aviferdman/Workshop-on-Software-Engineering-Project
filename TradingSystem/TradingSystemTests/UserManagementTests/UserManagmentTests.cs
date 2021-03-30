using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingSystem.Business.UserManagement;
namespace TradingSystemTests
{
    [TestClass]
    public class UserManagmentTests
    {
        private string signup()
        {
           return UserManagement.Instance.SignUp("inbi2001", "123456", "lalal 38, lala");
        }

        private bool delete()
        {
            return UserManagement.Instance.DeleteUser("inbi2001");
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        public void TestSignUpSuccess()
        {
            Assert.AreEqual("success", signup());
            delete();
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.SignUp(string, string, string)"/>
        [TestMethod]
        public void TestSignUpFail()
        {
            signup();
            Assert.AreNotEqual("success", signup());
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        public void TestLoginSuccess()
        {
            signup();
            Assert.AreEqual("success", UserManagement.Instance.LogIn("inbi2001", "123456"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// already logged in
        [TestMethod]
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
        public void TestLoginFailed2()
        {
            signup();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            delete();

        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        /// username doesn't exist
        [TestMethod]
        public void TestLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", UserManagement.Instance.LogIn("inbi2001", "12345d6"));
            delete();

        }
    }
}
