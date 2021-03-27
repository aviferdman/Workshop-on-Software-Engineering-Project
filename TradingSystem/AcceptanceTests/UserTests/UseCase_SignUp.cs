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
    [TestFixture(USERNAME, PASSWORD)]
    public class UseCase_SignUp : MemberUseTestBase
    {
        private readonly string username;
        private readonly string password;

        public UseCase_SignUp(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, Bridge.AssureSignUp(username, password));
        }

        [Test]
        public void Failure_UsernameTaken()
        {
            Success_Normal();
            Assert.AreEqual(true, Bridge.SignUp(username, "abcd1234"));
        }
    }
}