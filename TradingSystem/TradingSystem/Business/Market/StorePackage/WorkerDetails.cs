using System;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StorePackage
{
    class WorkerDetails
    {
        String Username;
        State State;
        List<Permission> permissions;

        public string Username1 { get => Username; set => Username = value; }
        public State State1 { get => State; set => State = value; }
        public List<Permission> Permissions { get => permissions; set => permissions = value; }

        public String toString()
        {
            String ret = "username: " + Username + "\nPermissions: ";
            foreach (Permission permission in permissions)
            {
                ret += permission.ToString() + ", ";
            }
            return ret;
        }
    }
}
