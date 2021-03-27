using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge) { }

        public override IMarketBridge Bridge => this;

        public int OpenShop(string shopName)
        {
            return RealBridge == null ? -1 : RealBridge.OpenShop(shopName);
        }
    }
}
