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
        public string Username { get; set; }
        public string Position { get; set; }
        public ICollection<string> Permissions { get; set; }

        public WorkerDetails(Manager manager)
        {
            Username = manager.username;
            Position = "manager";
            Permissions = manager.store_permission.Select(p=> p.p).ToList();
        }
        public WorkerDetails(Owner owner)
        {
            Username = owner.Username;
            Position = "owner";
            Permissions = new LinkedList<string>();
        }
        public WorkerDetails(Founder founder)
        {
            Username = founder.Username;
            Position = "founder";
            Permissions = new LinkedList<string>();
        }

        public bool Equals(WorkerDetails other)
        {
            return this.Username.Equals(other.Username) && this.Position.Equals(other.Position) && EqualPermissions(this.Permissions, other.Permissions);
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
