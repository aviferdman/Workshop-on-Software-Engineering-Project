using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface.UserBridge;

namespace AcceptanceTests.AppInterface
{
    /// <summary>
    /// It seems redundant now and it's too much of a hassle to change it now.
    /// 
    /// Was initially for preserving the state within a single test's execution
    /// such that other sub-tests it calls will remember this state.
    /// Running a new test will not interfere with this state (not as a sub-test),
    /// so we are not relaying on certain global state or execution order.
    /// Also, when executing a test, the actual system is likely to also keeps similar state,
    /// so were are not actualy saving state when there is no need to.
    /// </summary>
    public class SystemContext
    {
        private readonly ProxyBase<IUserBridge> userBridge;
        private readonly ProxyBase<IMarketBridge> marketBridge;

        private SystemContext(ProxyBase<IUserBridge> userBridge, ProxyBase<IMarketBridge> marketBridge)
        {
            this.userBridge = userBridge;
            this.marketBridge = marketBridge;
            LoggedInUser = null;
        }

        private static SystemContext New()
        {
            var systemContext = new SystemContext(
                userBridge: new UserBridgeProxy(),
                marketBridge: new MarketBridgeProxy()
            );
            systemContext.userBridge.SystemContext = systemContext;
            systemContext.userBridge.RealBridge = UserBridgeAdapter.New(systemContext);
            systemContext.marketBridge.SystemContext = systemContext;
            systemContext.marketBridge.RealBridge = MarketBridgeAdapter.New(systemContext);
            return systemContext;
        }

        public static SystemContext Instance { get; } = New();

        public IUserBridge UserBridge => userBridge.Bridge;
        public IMarketBridge MarketBridge => marketBridge.Bridge;
        internal string? TokenUsername { get; set; }
        public UserInfo? LoggedInUser { get; internal set; }
    }
}
