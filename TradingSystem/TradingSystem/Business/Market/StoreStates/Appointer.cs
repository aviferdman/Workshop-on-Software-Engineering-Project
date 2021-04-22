using System;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public interface Appointer
    {
        //appoint a new manager with memberStaste m to Store s adds to lists in store and memberState
        //use locks for store premmissions and memberState premissions
        public Manager AddAppointmentManager(MemberState m, Store s);
        public MemberState getM();

        //appoint a new owner with memberStaste m to Store s adds to lists in store and memberState
        //use locks for store premmissions and memberState premissions
        public Owner AddAppointmentOwner(MemberState m, Store s);

        /// checks if this user can remove the appointment of a user and if he does removes him from appointments list
        /// please notice that when a user is removed all his appointments should be removed and this function doesn't do that so take care of it
        /// if returns true need to remove appointment from userToRemove memberState's lists of premmissions, and from store's lists of premmissions
        ///use locks for store premmissions
        public bool canRemoveAppointment(string userToRemove);

        //define premissions to manger with matching username if manger has not been appointer by this appointer an UnauthorizedAccessException will be thrown
        public void DefinePermissions(string username, List<Permission> permissions);
        
    }
}
