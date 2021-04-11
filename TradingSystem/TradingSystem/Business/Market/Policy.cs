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

        public bool Check(Dictionary<Product, int> product_quantity)
        {
            bool isLegal = true;
            foreach (IRule rule in _rules)
            {
                isLegal = isLegal && rule.Check(product_quantity);
            }
            return isLegal;
        }
    }
}
