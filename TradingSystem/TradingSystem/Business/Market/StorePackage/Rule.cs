using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Rule : IRule
    {
        //Function gets a products and it's quantities and return legal or not
        Func<IShoppingBasket, bool> _r;

        public Rule(Func<IShoppingBasket, bool> r)
        {
            this._r = r;
        }

        //All products and it's quantities needs to pass the rule, otherwise its illegal
        virtual public bool Check(IShoppingBasket shoppingBasket)
        {
            return _r(shoppingBasket);
        }
    }
}
