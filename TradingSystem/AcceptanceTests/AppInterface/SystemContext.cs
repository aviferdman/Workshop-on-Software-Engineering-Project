﻿using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface.UserBridge;

namespace AcceptanceTests.AppInterface
{
    /// <summary>
    /// Preserves the state within a single test's execution
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

        private SystemContext()
        {
            userBridge = new UserBridgeProxy(null);
            marketBridge = new MarketBridgeProxy(null);
            LoggedInUser = null;
        }

        public static SystemContext New()
        {
            var system = new SystemContext();
            system.userBridge.System = system;
            system.marketBridge.System = system;
            return system;
        }

        public IUserBridge UserBridge => userBridge.Bridge;
        public IMarketBridge MarketBridge => marketBridge.Bridge;
        public UserInfo? LoggedInUser { get; internal set; }
    }
}
