using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.UserBridge;

using NUnit.Framework;

namespace AcceptanceTests
{
    public class MemberUseTestBase
    {
        public const string USERNAME = "user123";
        public const string PASSWORD = "mypassword1";

        protected SystemContext SystemContext { get; }
        public UserInfo UserInfo { get; }

        public MemberUseTestBase(SystemContext systemContext, UserInfo loginInfo)
        {
            SystemContext = systemContext;
            UserInfo = loginInfo;
        }

        protected IUserBridge Bridge => SystemContext.UserBridge;

        [SetUp]
        public virtual void Setup() { }
    }
}
