using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<string> MakeOwnerAsync(string assignee, Guid storeID, string assigner)
        {
            return await marketStores.makeOwner(assignee, storeID, assigner);
        }

        public async Task<string> MakeManagerAsync(string assignee, Guid storeID, string assigner)
        {
            return await marketStores.makeManager(assignee, storeID, assigner);
        }

        public async Task<string> DefineManagerPermissionsAsync(string manager, Guid storeID, string assigner, List<Permission> permissions)
        {
            return await marketStores.DefineManagerPermissions(manager, storeID, assigner, permissions);
        }

        public async Task<string> RemoveManagerAsync(String managerName, Guid storeID, String assignerName)
        {
            return await marketStores.RemoveManager(managerName, storeID, assignerName);
        }

        public async Task<string> RemoveOwnerAsync(String ownerName, Guid storeID, String assignerName)
        {
            return await marketStores.RemoveOwner(ownerName, storeID, assignerName);
        }

        public String getInfo(Guid storeID, String username)
        {
            return marketStores.getInfo(storeID, username);
        }

        public String getInfoSpecific(Guid storeID, String workerName, String username)
        {
            return marketStores.getInfoSpecific(storeID, workerName, username);
        }
    }
}
