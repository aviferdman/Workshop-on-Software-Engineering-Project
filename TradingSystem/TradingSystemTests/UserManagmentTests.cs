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
    }
}
