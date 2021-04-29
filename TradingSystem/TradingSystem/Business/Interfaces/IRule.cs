using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public interface IRule
    {
        public bool Check(IShoppingBasket shoppingBasket);
        public Guid GetId();
        Rule AndRules(IRule additionalRule);

    }
}