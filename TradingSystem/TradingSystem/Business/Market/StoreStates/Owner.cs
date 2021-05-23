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

        public Owner()
        {
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
                ret = m.ownerPrems.Remove(this) | appointer.ownerAppointments.Remove(this);
            }
            return ret;
        }

        public bool hasAppointees()
        {
            return s.owners.Where(m => m.appointer.username.Equals(this.username)).Any();
        }

        public void removeManagers()
        {
            List<Manager> mangersToRem = new List<Manager>();
            foreach (Manager manager in s.managers.Where(m=> m.appointer.username.Equals(this.username)))
            {
                mangersToRem.Add(manager);
            }
            foreach (Manager manager in mangersToRem)
            {
                s.RemoveManager(manager.Username, Username);
            }
        }

        public ICollection<String> getAppointees()
        {
            return s.owners.Where(m => m.appointer.username.Equals(this.username)).Select(o=> o.username).ToList();
        }
    }
}
