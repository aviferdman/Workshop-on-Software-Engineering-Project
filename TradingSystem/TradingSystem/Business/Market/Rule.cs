using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Rule
    {
        //Function gets a products and it's quantities and return legal or not
        Func<Dictionary<Product, int>, bool> _r;

        public Rule(Func<Dictionary<Product, int>, bool> r)
        {
            this._r = r;
        }

        //All products and it's quantities needs to pass the rule, otherwise its illegal
        public bool Check(Dictionary<Product, int> product_quantity)
        {
            return _r(product_quantity);
        }
    }
}
