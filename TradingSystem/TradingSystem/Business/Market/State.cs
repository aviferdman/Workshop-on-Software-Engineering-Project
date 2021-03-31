using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public abstract class State
    {
        private static IStorePermission _storePermision;
        protected static readonly Transaction _transaction = Transaction.Instance;

        public State(IStorePermission storePermission)
        {
            _storePermision = storePermission;
        }
        abstract public Store CreateStore(string shopName, BankAccount bank, Address address);

        abstract public History GetUserHistory(Guid userId);

        abstract public History GetAllHistory();

        abstract public History GetStoreHistory(Guid storeId);

        abstract public bool AddSubject(Guid storeId, Permission permission, IStorePermission subjectStorePermission);

        abstract public bool RemoveSubject(Guid storeId, IStorePermission subjectStorePermission);

        public Transaction GetTransaction()
        {
            return _transaction;
        }

        public IStorePermission GetStorePermission()
        {
            return _storePermision;
        }
    }
}
