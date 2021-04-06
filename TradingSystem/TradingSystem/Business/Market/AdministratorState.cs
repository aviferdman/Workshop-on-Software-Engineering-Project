using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

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

        //use case 40 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/66
        public override History GetUserHistory(Guid userId)
        {
            return GetTransaction().GetHistory(userId);
        }
    }
}
