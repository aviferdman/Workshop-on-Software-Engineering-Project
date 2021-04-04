using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
        public UseCase_SignUp(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
        public UseCase_SignUp(SystemContext systemContext, UserInfo signupInfo) :
            base(systemContext, signupInfo)
        { }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, Bridge.AssureSignUp(UserInfo));
        }

        [Test]
        public void Failure_UsernameTaken()
        {
            Success_Normal();
            Assert.AreEqual(true, Bridge.SignUp(UserInfo.WithDifferentPassword("abcd1234")));
        }
    }
}