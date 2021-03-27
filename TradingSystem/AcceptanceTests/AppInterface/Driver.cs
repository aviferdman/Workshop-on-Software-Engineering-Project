using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.User;

namespace AcceptanceTests.AppInterface
{
    public static class Driver
    {
        public static IUserBridge UserBridge => UserBridgeInitializer.UserBridge;

        // Defend against parallized initialization
        private static class UserBridgeInitializer
        {
            internal static IUserBridge UserBridge { get; } = new UserBridgeProxy(null);
        }
    }
}
