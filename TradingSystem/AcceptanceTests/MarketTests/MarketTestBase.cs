using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.MarketBridge;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    public class MarketTestBase
    {
        public const string USER_SHOP_OWNER_NAME = "shopowner78";
        public const string USER_SHOP_OWNER_PASSWORD = "shopowner_pass78";

        public const string USER_BUYER_NAME = "buyer78";
        public const string USER_BUYER_PASSWORD = "buyer_pass78";

        protected IMarketBridge Bridge { get; }
        public SystemContext SystemContext { get; }

        public MarketTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
            Bridge = systemContext.MarketBridge;
        }

        [SetUp]
        public virtual void Setup() { }
    }
}
