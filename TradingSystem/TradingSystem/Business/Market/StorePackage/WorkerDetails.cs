using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StorePackage
{
    public class WorkerDetails
    {
        String username;
        String position;
        ICollection<string> permissions;


        public WorkerDetails(Manager manager)
        {
            username = manager.Username;
            position = "manager";
            permissions = manager.Store_permission;
        }
        public WorkerDetails(Owner owner)
        {
            username = owner.Username;
            position = "owner";
            permissions = new LinkedList<string>();
        }
        public WorkerDetails(Founder founder)
        {
            username = founder.Username;
            position = "founder";
            permissions = new LinkedList<string>();
        }
    }
}
