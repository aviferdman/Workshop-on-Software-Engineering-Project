using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;

namespace TradingSystem.Business.Market
{
    public class Rule : IRule
    {
        //Function gets a products and it's quantities and return legal or not
        Func<ShoppingBasket, bool> _r;
        Guid id;

        public Rule(Func<ShoppingBasket, bool> r)
        {
            this._r = r;
            this.id = Guid.NewGuid();
        }

        public Rule AndRules(IRule additionalRule)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) => Check(basket) && additionalRule.Check(basket)));
        }

        public Rule OrRules(IRule additionalRule)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) => Check(basket) && additionalRule.Check(basket)));
        }

        //All products and it's quantities needs to pass the rule, otherwise its illegal
        public virtual bool Check(ShoppingBasket shoppingBasket)
        {
            var v = _r.Method.GetMethodBody();
            return _r(shoppingBasket);
        }

        public Guid GetId()
        {
            return id;
        }

        public static Rule AddTwoRules(IRule rule1, IRule rule2)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) => rule1.Check(basket) && rule2.Check(basket)));
        }
        public static Rule OrTwoRules(IRule rule1, IRule rule2)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) => rule1.Check(basket) || rule2.Check(basket)));
        }
        public static Rule CompositeTwoRules(IRule rule1, IRule rule2)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) => rule1.Check(basket) ? rule2.Check(basket) : true));
        }


    }
}
