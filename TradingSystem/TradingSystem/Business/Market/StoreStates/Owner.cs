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
            m.ManagerAppointments.TryAdd(username, prem);
            return prem;
        }
        public Owner AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem = makeOwner(m,s, this);
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
