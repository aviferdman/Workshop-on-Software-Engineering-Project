using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.User
{
    /// <summary>
    /// Acceptance test for
    /// Use case 2: Login
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_Login : MemberUseTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User,
            },
        };

        internal UseCase_SignUp test_signUp;

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

        public override void Teardown()
        {
            _ = Bridge.LogOut();
            test_signUp.Teardown();
            base.Teardown();
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
            Assert.AreEqual(false, Bridge.Login(new UserInfo("invaliduser1", "wrongpasswrod", "0501478963", new Address
            {
                State = "Israel",
                City = "City 2",
                Street = "Hello",
                ApartmentNum = "5",
            })));
        }

        [Test]
        public void Failure_AlreadyLoggedIn()
        {
            Success_Normal();
            Assert.AreEqual(false, Bridge.Login(UserInfo));
        }
    }
}
