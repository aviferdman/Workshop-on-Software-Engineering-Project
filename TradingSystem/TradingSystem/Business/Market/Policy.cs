using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Policy
    {
        private ICollection<Rule> _rules;

        public Policy()
        {
            this._rules = new HashSet<Rule>();
        }
        public Policy(ICollection<Rule> rules)
        {
            this._rules = rules;
        }

        public ICollection<Rule> Rules { get => _rules; set => _rules = value; }

        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }
        
        public void RemoveRule(Rule rule)
        {
            _rules.Remove(rule);
        }

        public bool Check(Dictionary<Product, int> product_quantity)
        {
            bool isLegal = true;
            foreach (Rule rule in _rules)
            {
                isLegal = isLegal && rule.Check(product_quantity);
            }
            return isLegal;
        }
    }
}
