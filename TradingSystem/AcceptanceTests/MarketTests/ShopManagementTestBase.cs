using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.MarketTests
{
    public class ShopManagementTestBase : MarketTestBase
    {
        public ShopManagementTestBase(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext)
        {
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; }
    }
}
