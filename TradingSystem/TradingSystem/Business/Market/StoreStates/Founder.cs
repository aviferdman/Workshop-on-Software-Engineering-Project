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
            m.ManagerAppointments.TryAdd(username, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem = Owner.makeOwner(m,s , this);
            m.OwnerAppointments.TryAdd(username, prem);
            return prem;
        }

        
        public bool canRemoveAppointment(string userToRemove)
        {
            Manager man;
            Owner o;
            return m.ManagerAppointments.TryRemove(userToRemove, out man) || m.OwnerAppointments.TryRemove(userToRemove, out o);
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
