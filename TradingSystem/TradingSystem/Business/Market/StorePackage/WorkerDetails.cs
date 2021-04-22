using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StorePackage
{
    class WorkerDetails
    {
        String username;
        ICollection<Permission> permissions;


        public WorkerDetails(Manager manager)
        {
            username = manager.Username;
            permissions = manager.Store_permission;
        }

        public String toString()
        {
            String ret = "Manager - " + username + "\n\tPermissions: ";
            foreach (Permission permission in permissions)
            {
                ret += permission.ToString() + ", ";
            }
            return ret;
        }
    }
}
