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
    [TestFixture("signup_user123", "mypassword1")]
    public class UseCase_SignUp
    {
        private readonly string username;
        private readonly string password;

        private IUserBridge bridge;

        public UseCase_SignUp(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        [SetUp]
        public void Setup()
        {
            bridge = Driver.UserBridge();
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, bridge.SignUp(username, password));
        }

        [Test]
        public void Failure_UsernameTaken()
        {
            Success_Normal();
            Assert.AreEqual(true, bridge.SignUp(username, "abcd1234"));
        }
    }
}