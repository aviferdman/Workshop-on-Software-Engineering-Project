using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.MarketTests
{
    public class BuyerTestBase : MarketTestBase
    {
        public BuyerTestBase(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext)
        {
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; }
    }
}
