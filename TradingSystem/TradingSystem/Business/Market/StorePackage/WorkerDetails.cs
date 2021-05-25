using System;
using System.Collections.Generic;
using System.Linq;
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
            username = manager.username;
            position = "manager";
            permissions = manager.store_permission.Select(p=> p.p).ToList();
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

        public bool Equals(WorkerDetails other)
        {
            return this.username.Equals(other.username) && this.position.Equals(other.position) && EqualPermissions(this.permissions, other.permissions);
        }

        public bool EqualPermissions(ICollection<String> one, ICollection<String> other)
        {
            if (one.Count != other.Count)
                return false;
            foreach (String permission in one)
            {
                if (!other.Contains(permission))
                    return false;
            }
            return true;
        }
    }
}
