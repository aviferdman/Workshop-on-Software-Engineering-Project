﻿using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class GuestState : State
    {
        public GuestState() : base()
        {

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

    }
}
