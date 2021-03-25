using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Market
    {
        private ICollection<Store> stores;
        private ICollection<User> users;
        private static readonly Lazy<Market>
        lazy =
        new Lazy<Market>
            (() => new Market());

        public static Market Instance { get { return lazy.Value; } }

        private Market()
        {
        }
    }
}
