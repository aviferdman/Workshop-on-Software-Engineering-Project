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
        [TestCase("user123", "mypassword1")]
        public void Success_Normal(string username, string password)
        {
            Assert.AreEqual(true, bridge.SignUp(username, password));
        }

        [Test]
        public void Failure_UsernameTaken()
        {
            Success_Normal("user123", "mypassword1");
            Assert.AreEqual(true, bridge.SignUp("user123", "abcd1234"));
        }
    }
}