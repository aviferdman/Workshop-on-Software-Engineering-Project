using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        private Guid _id;
        public StoreData(Store store)
        {
            this._id = store.Id;
        }
    }
}
