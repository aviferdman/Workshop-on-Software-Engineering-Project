
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.MarketBridge;

namespace AcceptanceTests.MarketTests
{
    public class MarketTestBase
    {
        public const string USER_SHOP_OWNER_NAME = SharedTestsConstants.USER_SHOP_OWNER_NAME;
        public const string USER_SHOP_OWNER_PASSWORD = SharedTestsConstants.USER_SHOP_OWNER_PASSWORD;

        public const string USER_SHOP_OWNER_2_NAME = SharedTestsConstants.USER_SHOP_OWNER_2_NAME;
        public const string USER_SHOP_OWNER_2_PASSWORD = SharedTestsConstants.USER_SHOP_OWNER_2_PASSWORD;

        public const string USER_BUYER_NAME = SharedTestsConstants.USER_BUYER_NAME;
        public const string USER_BUYER_PASSWORD = SharedTestsConstants.USER_BUYER_PASSWORD;

        public const string SHOP_NAME = SharedTestsConstants.SHOP_NAME;
        public const string SHOP_NAME_2 = SharedTestsConstants.SHOP_NAME_2;

        public SystemContext SystemContext { get; }

        public MarketTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        protected IMarketBridge Bridge => SystemContext.MarketBridge;
    }
}
