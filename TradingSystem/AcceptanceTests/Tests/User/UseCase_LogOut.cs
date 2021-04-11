using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.User
{
    /// <summary>
    /// Acceptance test for
    /// Use case 3: Log out
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/51
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_LogOut : MemberUseTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User,
            },
        };

        private UseCase_Login test_login;
        private UseCase_LogOut_TestLogic testLogic;
        private bool logged_out;

        public UseCase_LogOut(SystemContext systemContext, UserInfo loginInfo) :
            base(systemContext, loginInfo)
        { }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            test_login = new UseCase_Login(SystemContext, UserInfo);
            test_login.Setup();
            test_login.Success_Normal();

            testLogic = new UseCase_LogOut_TestLogic(SystemContext);
            logged_out = false;
        }

        public override void Teardown()
        {
            if (logged_out)
            {
                test_login.test_signUp.Teardown();
            }
            else
            {
                test_login.Teardown();
            }
            base.Teardown();
        }

        [Test]
        public void Success_Normal()
        {
            testLogic.Success_Normal();
            logged_out = true;
        }
    }
}
