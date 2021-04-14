using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;

namespace AcceptanceTests.Tests.Market
{
    public class MarketTestBase
    {
        public static readonly UserInfo User_Buyer = SharedTestsData.User_Buyer;
        public static readonly UserInfo User_ShopOwner1 = SharedTestsData.User_ShopOwner1;
        public static readonly UserInfo User_ShopOwner2 = SharedTestsData.User_ShopOwner2;

        public static readonly ShopInfo Shop1 = SharedTestsData.Shop1;
        public static readonly ShopInfo Shop2 = SharedTestsData.Shop2;

        public SystemContext SystemContext { get; }

        public MarketTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        protected IMarketBridge Bridge => SystemContext.MarketBridge;
    }
}
