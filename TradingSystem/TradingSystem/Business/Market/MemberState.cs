using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        private Guid _userId;
        private IStorePermission _storePermission;

        public MemberState(Guid userId, IStorePermission storePermission) : base(storePermission)
        {
            this._storePermission = storePermission;
            this._userId = userId;
        }

        public override Store CreateStore(string shopName, BankAccount bank, Address address)
        {
            Store store = new Store(shopName, bank, address);
            _storePermission.AddFounder(store.Id);
            return store;
        }

        public override bool AddSubject(Guid storeId, Permission permission, IStorePermission subjectStorePermission)
        {
            return _storePermission.AddSubject(storeId, permission, subjectStorePermission);
        }

        public override bool RemoveSubject(Guid storeId, IStorePermission subjectStorePermission)
        {
            return _storePermission.RemoveSubject(storeId, subjectStorePermission);
        }

        public override History GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override History GetUserHistory(Guid userId)
        {
            if (!_storePermission.GetUserHistory(_userId))
            {
                throw new UnauthorizedAccessException();
            }
            Transaction transaction = GetTransaction();
            return transaction.GetHistory(userId);
        }

        public override History GetStoreHistory(Guid storeId)
        {
            if (!_storePermission.GetStoreHistory(storeId))
            {
                throw new UnauthorizedAccessException();
            }
            return GetTransaction().GetStoreHistory(storeId);

        }

    }
}
