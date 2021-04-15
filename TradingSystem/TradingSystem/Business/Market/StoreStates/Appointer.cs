using System;
using System.Collections.Generic;
using System.Text;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public interface Appointer
    {
        //appoint a new manager with username username
        public Manager AddAppointmentManager(string username);

        //appoint a new owner with username username
        public Owner AddAppointmentOwner(string username);

        /// checks if this user can remove the appiontment of a user and if he does removes him from appointments list
        /// please notice that when a user is removed all his appointments should be removed and this function d
        public bool canRemoveAppointment(string userToRemove);

        //add new premission to manger with matching username if manger has not been appointer by this appointer an UnauthorizedAccessException will be thrown
        public void AddPermission(string username, Permission permission);

        //removes premission from manger with matching username if manger has not been appointer by this appointer an UnauthorizedAccessException will be thrown
        public void RemovePermission(string username, Permission permission);
        
    }
}
