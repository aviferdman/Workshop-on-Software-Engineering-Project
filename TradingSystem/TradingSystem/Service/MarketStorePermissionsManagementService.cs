﻿using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

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

        public string DefineManagerPermissions(string manager, Guid storeID, string assigner, List<Business.Market.StoreStates.Manager.Permission> permissions)
        {
            return marketStores.DefineManagerPermissions(manager, storeID, assigner, permissions);
        }
    }
}
