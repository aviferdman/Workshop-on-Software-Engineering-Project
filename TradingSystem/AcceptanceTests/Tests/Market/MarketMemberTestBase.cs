using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

namespace AcceptanceTests.Tests.Market
{
    public class MarketMemberTestBase : MarketTestBase
    {
        public UserInfo UserInfo { get; }

        public MarketMemberTestBase(SystemContext systemContext, UserInfo userInfo)
            : base(systemContext)
        {
            UserInfo = userInfo;
        }

        protected UseCase_Login Login()
        {
            return Login(UserInfo);
        }
    }
}
