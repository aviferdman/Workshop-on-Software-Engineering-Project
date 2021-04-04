using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.UserBridge;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests
{
    public class MemberUseTestBase : UserTestBase
    {
        public const string USERNAME = SharedTestsConstants.USER_OTHER_NAME;
        public const string PASSWORD = SharedTestsConstants.USER_OTHER_PASSWORD;

        public UserInfo UserInfo { get; }

        public MemberUseTestBase(SystemContext systemContext, UserInfo loginInfo)
            : base(systemContext)
        {
            UserInfo = loginInfo;
        }

        [SetUp]
        public virtual void Setup() { }
    }
}
