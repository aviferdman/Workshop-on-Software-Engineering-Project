using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Policy
    {
        private ICollection<IRule> _rules;

        public Policy()
        {
            this._rules = new HashSet<IRule>();
        }

        public Policy(ICollection<IRule> rules)
        {
            this._rules = rules;
        }

        public ICollection<IRule> Rules { get => _rules; set => _rules = value; }

        public void AddRule(IRule rule)
        {
            _rules.Add(rule);
        }
        
        public void RemoveRule(IRule rule)
        {
            _rules.Remove(rule);
        }

        public bool Check(IShoppingBasket shoppingBasket)
        {
            bool isLegal = true;
            foreach (IRule rule in _rules)
            {
                isLegal = isLegal && rule.Check(shoppingBasket);
            }
            return isLegal;
        }

        public Policy And(Policy additionalPolicy)
        {
            ICollection<IRule> andRules = new HashSet<IRule>();
            IRule and = new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket shoppingBasket) => Check(shoppingBasket) && additionalPolicy.Check(shoppingBasket)));
            andRules.Add(and);
            return new Policy(andRules);
        }

        public Policy Or(Policy orPolicy)
        {
            ICollection<IRule> orRules = new HashSet<IRule>();
            IRule or = new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket shoppingBasket) => Check(shoppingBasket) || orPolicy.Check(shoppingBasket)));
            orRules.Add(or);
            return new Policy(orRules);
        }
    }
}
