using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public abstract class Appointer
    {
        public MemberState m { get; set; }
        public Store s { get; set; }

        public Guid sid { get; set; }
        public string username { get; set; }

        public Appointer()
        {
        }

        //appoint a new manager with memberStaste m to Store s adds to lists in store and memberState
        //use locks for store premmissions and memberState premissions
        public async  Task<Manager> AddAppointmentManager(MemberState m, Store s)
        {
            Manager prem = Manager.makeManager(m, s, this);
            await ProxyMarketContext.Instance.saveChanges();
            return prem;
        }
        public abstract MemberState getM();

        //appoint a new owner with memberStaste m to Store s adds to lists in store and memberState
        //use locks for store premmissions and memberState premissions
       
        public async Task<Owner> AddAppointmentOwner(MemberState m, Store s)
        {
            Owner prem;
            lock (m)
            {
                prem = Owner.makeOwner(m, s, this);
            }
           
            await ProxyMarketContext.Instance.saveChanges();
            return prem;
        }

        /// checks if this user can remove the appointment of a user and if he does removes him from appointments list
        /// please notice that when a user is removed all his appointments should be removed and this function doesn't do that so take care of it
        /// if returns true need to remove appointment from userToRemove memberState's lists of premmissions, and from store's lists of premmissions
        ///use locks for store premmissions
        public bool canRemoveAppointment(string userToRemove)
        {
            return s.managers.Where(m => m.appointer.username.Equals(username) && m.username.Equals( userToRemove)).Any() || s.owners.Where(m => m.appointer.username.Equals(username) && m.username.Equals(userToRemove)).Any();
        }


        //define premissions to manger with matching username if manger has not been appointer by this appointer an UnauthorizedAccessException will be thrown
        public async Task DefinePermissions(string username, Manager man, List<Permission> permissions)
        {
            if (!man.appointer.username.Equals(this.username))
                throw new UnauthorizedAccessException();
            List<Prem> perms = new List<Prem>();
            foreach(Permission p in permissions)
            {
                perms.Add(new Prem(p.ToString()));
            }
            man.store_permission = perms;
            await ProxyMarketContext.Instance.saveChanges();
        }
    }
}
