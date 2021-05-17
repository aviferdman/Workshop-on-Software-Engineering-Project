using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Interfaces
{
    public interface IManager
    {
        public bool GetPermission(Permission permission);
        public bool removePermission(Store store);
        public MemberState getM();
    }
}
