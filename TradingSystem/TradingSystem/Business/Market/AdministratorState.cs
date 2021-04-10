using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class AdministratorState : MemberState
    {
        public AdministratorState(Guid userId) : base(userId)
        {

        }

        public override History GetAllHistory()
        {
            return GetTransaction().GetAllHistory();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public override History GetStoreHistory(Guid storeId)
        {
            return GetTransaction().GetStoreHistory(storeId);
        }

        //use case 40 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/66
        public override History GetUserHistory(Guid userId)
        {
            return GetTransaction().GetHistory(userId);
        }
    }
}
