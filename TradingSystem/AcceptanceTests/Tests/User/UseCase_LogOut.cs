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
    [TestFixture(USERNAME, PASSWORD)]
    public class UseCase_LogOut : MemberUseTestBase
    {
        private UseCase_Login test_login;
        private UseCase_LogOut_TestLogic testLogic;

        public UseCase_LogOut(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
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
        }

        [Test]
        public void Success_Normal()
        {
            testLogic.Success_Normal();
        }
    }
}
