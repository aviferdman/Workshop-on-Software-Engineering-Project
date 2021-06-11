using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Service
{
    public class MarketStorePermissionsManagementService
    {
        
        private readonly MarketStores marketStores;

        public MarketStorePermissionsManagementService(MarketStores marketStores)
        {
            this.marketStores = marketStores;
        }

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

        public async Task<ICollection<WorkerDetails>> GetInfo(Guid storeID, String username)
        {
            return await marketStores.GetInfo(storeID, username);
        }

        public async Task<WorkerDetails> GetInfoSpecific(Guid storeID, String workerName, String username)
        {
            return await marketStores.GetInfoSpecific(storeID, workerName, username);
        }

        public async Task<Result<WorkerDetails>> GetPrems(Guid storeID, String username)
        {
            return await marketStores.GetPerms(storeID, username);
        }
    }
}
