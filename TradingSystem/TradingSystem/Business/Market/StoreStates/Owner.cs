using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Owner:Appointer
    {
        
        public MemberState appointer { get; set; }

        public string Username { get => username; set => username = value; }
        public Store S { get => s; set => s = value; }

        private Owner(MemberState m, Store s, MemberState appointer)
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
            this.appointer = appointer;
            this.sid = sid;
        }

        public override MemberState getM() { return m; }

        public  static Owner makeOwner(MemberState m, Store s, Appointer appointer)
        {
            if (s.isStaff(m.UserId))
                throw new InvalidOperationException();
            Owner o = new Owner(m, s, appointer.getM());
            s.Owners.Add( o);
            return o;
        }




        public bool removePermission(Store store)
        {
            bool ret;
            lock (m)
            {
                ret = m.ownerPrems.TryRemove(store, out _) | appointer.ownerAppointments.TryRemove(username, out _);
            }
            return ret;
        }

        public bool hasAppointees()
        {
            return !m.ownerAppointments.IsEmpty;
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
