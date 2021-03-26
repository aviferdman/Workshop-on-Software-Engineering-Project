using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.User;

namespace AcceptanceTests.AppInterface
{
    public static class Driver
    {
        private static IUserBridge userBridge;

        static Driver()
        {
            userBridge = new UserBridgeProxy(null);
        }

        public static IUserBridge UserBridge()
        {
            return userBridge;
        }
    }
}
