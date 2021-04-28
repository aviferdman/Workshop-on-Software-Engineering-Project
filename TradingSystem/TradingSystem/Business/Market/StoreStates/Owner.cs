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
        private MemberState appointer;

        public string Username { get => username; set => username = value; }

        private Owner(MemberState m, Store s, MemberState appointer)
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
            this.appointer = appointer;
        }

        public MemberState getM() { return m; }

        public static Owner makeOwner(MemberState m, Store s, Appointer appointer)
        {
            if (m.isStaff(s) || s.isStaff(m.UserId))
                throw new InvalidOperationException();
            Owner o = new Owner(m, s, appointer.getM());
            m.OwnerPrems.TryAdd(s, o);
            s.Owners.TryAdd(m.UserId, o);
            return o;
        }

        public Manager AddAppointmentManager(MemberState m, Store s)
        {
            Manager prem = Manager.makeManager(m, s, this);
            this.m.ManagerAppointments.TryAdd(m.UserId, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem = makeOwner(m,s, this);
            this.m.OwnerAppointments.TryAdd(m.UserId, prem);
            return prem;
        }
        public bool canRemoveAppointment(string userToRemove)
        {
            return m.ManagerAppointments.ContainsKey(userToRemove) || m.OwnerAppointments.ContainsKey(userToRemove);
        }

        public bool removeAppointment(string userToRemove)
        {
            return m.ManagerAppointments.TryRemove(userToRemove, out _) || m.OwnerAppointments.TryRemove(userToRemove, out _);
        }

        public void DefinePermissions(string username, List<Permission> permissions)
        {
            Manager man;
            if (!m.ManagerAppointments.TryGetValue(username, out man))
                throw new UnauthorizedAccessException();
            man.Store_permission = permissions;
        }

        public bool removePermission(Store store)
        {
            bool ret;
            lock (m.Prem_lock)
            {
                ret = m.OwnerPrems.TryRemove(store, out _) | appointer.OwnerAppointments.TryRemove(Username, out _);
            }
            return ret;
        }

        public bool hasAppointees()
        {
            return !m.OwnerAppointments.IsEmpty;
        }

        public void removeManagers()
        {
            foreach (Manager manager in m.ManagerAppointments.Values)
            {
                s.RemoveManager(manager.Username, Username);
            }
        }

        public ICollection<String> getAppointees()
        {
            return m.OwnerAppointments.Keys;
        }
    }
}
