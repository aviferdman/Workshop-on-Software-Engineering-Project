using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.DAL
{
    class MarketUsersDAL
    {
        ProxyMarketContext proxyMarketContext = ProxyMarketContext.Instance;
        public static MarketUsersDAL Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketUsersDAL>
       _lazy =
       new Lazy<MarketUsersDAL>
           (() => new MarketUsersDAL());
    }
}
