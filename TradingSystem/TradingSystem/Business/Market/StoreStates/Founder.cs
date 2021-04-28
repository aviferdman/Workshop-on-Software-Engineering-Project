using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Founder : Appointer
    {

        private string username;
        private MemberState m;
        private Store s;
        

        public string Username { get => username; set => username = value; }
        public MemberState getM() { return m; }

        private Founder(MemberState m, Store s)
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
        }

        public static Founder makeFounder(MemberState m, Store s)
        {
            if (m.isStaff(s) || s.isStaff(m.UserId))
                throw new InvalidOperationException();
            return new Founder(m, s);
        }

        public Manager AddAppointmentManager(MemberState m, Store s)
        {
            Manager prem = Manager.makeManager(m,s,this);
            this.m.ManagerAppointments.TryAdd(m.UserId, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem = Owner.makeOwner(m,s , this);
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
    }
}
