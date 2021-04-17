using System;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Interfaces
{
    public interface IManager
    {
        public bool GetPermission(Permission permission);
    }
}
