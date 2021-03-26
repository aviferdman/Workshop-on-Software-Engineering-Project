using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.User;

using NUnit.Framework;

namespace AcceptanceTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 1: Sign Up
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/11
    /// </summary>
    public class UseCase_SignUp
    {
        private IUserBridge bridge;

        [SetUp]
        public void Setup()
        {
            bridge = Driver.UserBridge();
        }

        [Test]
        public void SignUp_Success_Normal()
        {
            Assert.AreEqual(true, bridge.SignUp("user123", "mypassword1"));
        }

        [Test]
        public void SignUp_Failure_UsernameTaken()
        {
            SignUp_Success_Normal();
            Assert.AreEqual(true, bridge.SignUp("user123", "abcd1234"));
        }
    }
}