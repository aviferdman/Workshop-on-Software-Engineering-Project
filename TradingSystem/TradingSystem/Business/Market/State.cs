﻿using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public abstract class State
    {
        protected static readonly Transaction _transaction = Transaction.Instance;

        public State()
        {
        }

        public abstract History GetUserHistory(Guid userId);

        public abstract History GetAllHistory();

        public Transaction GetTransaction()
        {
            return _transaction;
        }

    }
}
