using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;

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
