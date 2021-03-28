using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    abstract class State
    {
        protected static readonly Transaction _transaction = Transaction.Instance;

        abstract public bool CreateStore(string shopName);

        abstract public History GetUserHistory(Guid userId);

        abstract public History GetAllHistory();

        abstract public History GetStoreHistory(Guid storeId);

        abstract public bool AddSubject(Guid newManagerId, Guid storeId, Permission permission);

        abstract public bool RemoveSubject(Guid newManagerId, Guid storeId);

        public Transaction GetTransaction()
        {
            return _transaction;
        }
    }
}
