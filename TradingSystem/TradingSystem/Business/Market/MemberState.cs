using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class MemberState : State
    {
        private StorePermission _storePermission;
        private Guid _userId;


        public MemberState(Guid userId) : base()
        {
            this._storePermission = new StorePermission(userId);
            this._userId = userId;
        }

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public override bool CreateStore(string shopName)
        {
            Store store = new Store();
            _storePermission.AddFounder(_userId, store.Id);
            return true;
        }

        public override bool AddSubject(Guid newManagerId, Guid storeId, Permission permission)
        {
            return _storePermission.AddSubject(newManagerId, storeId, permission);
        }

        public override bool RemoveSubject(Guid managerId, Guid storeId)
        {
            return _storePermission.RemoveSubject(managerId, storeId);
        }

        public override History GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public override History GetUserHistory(Guid userId)
        {
            if (!_storePermission.GetUserHistory(_userId))
            {
                throw new UnauthorizedAccessException();
            }
            Transaction transaction = GetTransaction();
            return transaction.GetHistory(userId);
        }

        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public override History GetStoreHistory(Guid storeId)
        {
            if (!_storePermission.GetStoreHistory(_userId, storeId))
            {
                throw new UnauthorizedAccessException();
            }
            return GetTransaction().GetStoreHistory(storeId);

        }

    }
}
