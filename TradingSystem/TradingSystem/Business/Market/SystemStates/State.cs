using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public abstract class State
    {
        protected static readonly Transaction _transaction = Transaction.Instance;

        public State()
        {
        }

        public abstract UserHistory GetUserHistory(Guid userId);

        public abstract StoreHistory GetStoreHistory(Store store);

        public abstract ICollection<IHistory> GetAllHistory();

        public Transaction GetTransaction()
        {
            return _transaction;
        }

    }
}
