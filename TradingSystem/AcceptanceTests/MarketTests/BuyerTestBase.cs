using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;

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
