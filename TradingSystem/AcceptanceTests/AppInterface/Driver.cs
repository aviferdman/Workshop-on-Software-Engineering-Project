using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.User;

namespace AcceptanceTests.AppInterface
{
    public static class Driver
    {
        public static IUserBridge UserBridge()
        {
            return new UserBridgeProxy(null);
        }
    }
}
