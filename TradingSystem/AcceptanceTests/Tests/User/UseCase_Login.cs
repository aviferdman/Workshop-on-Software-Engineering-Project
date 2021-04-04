using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 2: Login
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21
    /// </summary>
    [TestFixture(USERNAME, PASSWORD)]
    public class UseCase_Login : MemberUseTestBase
    {
        private UseCase_SignUp test_signUp;
        public UseCase_Login(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
        public UseCase_Login(SystemContext systemContext, UserInfo loginInfo) :
            base(systemContext, loginInfo)
        { }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            test_signUp = new UseCase_SignUp(SystemContext, UserInfo);
            test_signUp.Setup();
            test_signUp.Success_Normal();
        }

        [TearDown]
        public void TearDown()
        {

            // Should we use even use assertions in a teardown?
            _ = Bridge.LogOut();
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, Bridge.Login(UserInfo));
        }

        [Test]
        public void Failure_PasswordMismatch()
        {
            Assert.AreEqual(false, Bridge.Login(UserInfo.WithDifferentPassword("wrongpasswrod")));
        }

        [Test]
        public void Failure_UsernameDoesntExist()
        {
            Assert.AreEqual(false, Bridge.Login(new UserInfo("invaliduser1", "wrongpasswrod")));
        }

        [Test]
        public void Failure_AlreadyLoggedIn()
        {
            Success_Normal();
            Assert.AreEqual(false, Bridge.Login(UserInfo));
        }
    }
}
