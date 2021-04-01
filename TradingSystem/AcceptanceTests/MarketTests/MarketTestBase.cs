
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.MarketBridge;

namespace AcceptanceTests.MarketTests
{
    public class MarketTestBase
    {
        public SystemContext SystemContext { get; }

        public MarketTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        protected IMarketBridge Bridge => SystemContext.MarketBridge;
    }
}
