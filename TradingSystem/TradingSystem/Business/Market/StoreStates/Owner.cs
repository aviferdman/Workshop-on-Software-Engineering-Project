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
        private MemberState m;
        private Store s;
        private ConcurrentDictionary<string, Manager> managerAppointments;
        private ConcurrentDictionary<string, Owner> ownerAppointments;
        private Appointer appointer;
        private Owner(MemberState m, Store s, Appointer appointer)
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
            this.appointer = appointer;
            managerAppointments = new ConcurrentDictionary<string, Manager>();
            ownerAppointments = new ConcurrentDictionary<string, Owner>();
        }

        public static Owner makeOwner(MemberState m, Store s, Appointer appointer)
        {
            if (m.isStaff(s) || s.isStaff(m.UserId))
                throw new InvalidOperationException();
            return new Owner(m, s, appointer);
        }

        public Manager AddAppointmentManager(MemberState m, Store s)
        {
            Manager prem = Manager.makeManager(m, s, this);
            managerAppointments.TryAdd(username, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem = makeOwner(m,s, this);
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
