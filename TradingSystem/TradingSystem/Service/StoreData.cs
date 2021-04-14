using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        public StoreData(Store store)
        {
            Id = store.Id;
        }

        public Guid Id { get; }
    }
}
