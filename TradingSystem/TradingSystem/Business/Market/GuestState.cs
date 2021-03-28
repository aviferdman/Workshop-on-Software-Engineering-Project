using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class GuestState : State
    {
        public GuestState()
        {

        }

        public override bool AddSubject(Guid newManagerId, Guid storeId, Permission permission)
        {
            throw new UnauthorizedAccessException();
        }

        public override bool CreateStore(string storeName)
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

        public override bool RemoveSubject(Guid newManagerId, Guid storeId)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
