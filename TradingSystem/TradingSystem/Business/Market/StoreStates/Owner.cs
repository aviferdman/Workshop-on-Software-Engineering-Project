using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Owner:Appointer
    {
        private string username;
        private ConcurrentDictionary<string, Manager> managerAppointments;
        private ConcurrentDictionary<string, Owner> ownerAppointments;
        private Appointer appointer;
        public Owner(string username, Appointer appointer)
        {
            this.username = username;
            this.appointer = appointer;
            managerAppointments = new ConcurrentDictionary<string, Manager>();
            ownerAppointments = new ConcurrentDictionary<string, Owner>();
        }

        public Manager AddAppointmentManager(string username)
        {
            Manager prem = new Manager(username, this);
            managerAppointments.TryAdd(username, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(string username)
        {
            Owner prem = new Owner(username, this);
            ownerAppointments.TryAdd(username, prem);
            return prem;
        }
        public bool canRemoveAppointment(string userToRemove)
        {
            Manager m;
            Owner o;
            return managerAppointments.TryRemove(userToRemove, out m) || ownerAppointments.TryRemove(userToRemove, out o);
        }

        public void AddPermission(string username, Permission permission)
        {
            Manager m;
            if (!managerAppointments.TryGetValue(username, out m))
                throw new UnauthorizedAccessException();
            m.AddPermission(permission);
        }


        public void RemovePermission(string username, Permission permission)
        {
            Manager m;
            if (!managerAppointments.TryGetValue(username, out m))
                throw new UnauthorizedAccessException();
            m.RemovePermission(permission);
        }
    }
}
