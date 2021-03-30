using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public abstract class State
    {
        private static StorePermission _storePermision;
        protected static readonly Transaction _transaction = Transaction.Instance;

        public State(StorePermission storePermission)
        {
            _storePermision = storePermission;
        }
        abstract public Store CreateStore(string shopName, BankAccount bank, Address address);

        abstract public History GetUserHistory(Guid userId);

        abstract public History GetAllHistory();

        abstract public History GetStoreHistory(Guid storeId);

        abstract public bool AddSubject(Guid storeId, Permission permission, StorePermission subjectStorePermission);

        abstract public bool RemoveSubject(Guid storeId, StorePermission subjectStorePermission);

        public Transaction GetTransaction()
        {
            return _transaction;
        }

        public StorePermission GetStorePermission()
        {
            return _storePermision;
        }
    }
}
