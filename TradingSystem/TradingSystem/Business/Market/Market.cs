using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Market
    {
        private ICollection<Store> _stores;
        private ICollection<User> _users;
        private static readonly Lazy<Market>
        _lazy =
        new Lazy<Market>
            (() => new Market());

        public static Market Instance { get { return _lazy.Value; } }

        private Market()
        {
        }
    }
}
