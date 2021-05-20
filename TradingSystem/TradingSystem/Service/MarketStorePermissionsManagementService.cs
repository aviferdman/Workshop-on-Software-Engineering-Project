using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Service
{
    public class MarketStorePermissionsManagementService
    {
        private static readonly Lazy<MarketStorePermissionsManagementService> instanceLazy =
            new Lazy<MarketStorePermissionsManagementService>(() => new MarketStorePermissionsManagementService(), true);

        private readonly MarketStores marketStores;

        private MarketStorePermissionsManagementService()
        {
            marketStores = MarketStores.Instance;
        }

        public static MarketStorePermissionsManagementService Instance => instanceLazy.Value;

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

        public String RemoveManager(String managerName, Guid storeID, String assignerName)
        {
            return marketStores.RemoveManager(managerName, storeID, assignerName);
        }

        public String RemoveOwner(String ownerName, Guid storeID, String assignerName)
        {
            return marketStores.RemoveOwner(ownerName, storeID, assignerName);
        }

        public String GetInfo(Guid storeID, String username)
        {
            return marketStores.GetInfo(storeID, username);
        }

        public String GetInfoSpecific(Guid storeID, String workerName, String username)
        {
            return marketStores.GetInfoSpecific(storeID, workerName, username);
        }
    }
}
