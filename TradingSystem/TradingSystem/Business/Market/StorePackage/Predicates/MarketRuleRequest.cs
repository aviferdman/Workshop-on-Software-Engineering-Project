using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market.StorePackage.Predicates
{
    public interface MarketRuleRequest
    {
        public int getCounter();
        public Task ActivateFunction();
    }
}
