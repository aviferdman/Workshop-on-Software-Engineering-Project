using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState(StorePermission storePermission) : base(storePermission)
        {

        }

        public override bool AddSubject(Guid storeId, Permission permission, StorePermission subjectStorePermission)
        {
            throw new UnauthorizedAccessException();
        }

        public override Store CreateStore(string storeName, BankAccount bank, Address address)
        {
            throw new UnauthorizedAccessException();
        }
        public override History GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override History GetStoreHistory(Guid storeId)
        {
            throw new UnauthorizedAccessException();
        }

        public override History GetUserHistory(Guid userId)
        {
            throw new UnauthorizedAccessException();
        }

        public override bool RemoveSubject(Guid storeId, StorePermission subjectStorePermission)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
