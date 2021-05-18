using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.DAL
{
    class MarketStoresDAL
    {
        ProxyMarketContext proxyMarketContext = ProxyMarketContext.Instance;
        public static MarketStoresDAL Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketStoresDAL>
       _lazy =
       new Lazy<MarketStoresDAL>
           (() => new MarketStoresDAL());
    }
}
