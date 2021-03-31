using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState(IStorePermission storePermission) : base(storePermission)
        {

        }

        public override bool AddSubject(Guid storeId, Permission permission, IStorePermission subjectStorePermission)
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

        public override bool RemoveSubject(Guid storeId, IStorePermission subjectStorePermission)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
