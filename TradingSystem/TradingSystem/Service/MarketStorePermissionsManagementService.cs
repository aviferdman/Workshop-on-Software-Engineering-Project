using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketStorePermissionsManagementService
    {
        private readonly MarketStores marketStores;

        public MarketStorePermissionsManagementService()
        {
            marketStores = MarketStores.Instance;
        }

        public string MakeOwner(string assignee, Guid storeID, string assigner)
        {
            return marketStores.makeOwner(assignee, storeID, assigner);
        }

        public string MakeManager(string assignee, Guid storeID, string assigner)
        {
            return marketStores.makeManager(assignee, storeID, assigner);
        }

        public string DefineManagerPermissions(string manager, Guid storeID, string assigner, List<Permission> permissions)
        {
            return marketStores.DefineManagerPermissions(manager, storeID, assigner, permissions);
        }
    }
}
