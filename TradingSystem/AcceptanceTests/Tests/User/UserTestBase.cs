
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.UserBridge;

namespace AcceptanceTests.Tests.User
{
    public class UserTestBase
    {
        protected SystemContext SystemContext { get; }

        public UserTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        protected IUserBridge Bridge => SystemContext.UserBridge;
    }
}
