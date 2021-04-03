using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Interfaces
{
    public interface IStorePermission
    {
        public bool GetPermission(Permission permission);
        public void AddPermission(Guid user, Permission permission);
        public void RemovePermission(Guid user, Permission permission);
        public StorePermission AddAppointment(Guid user, AppointmentType appointment);
        public bool canRemoveAppointment(Guid userToRemove);
    }
}
