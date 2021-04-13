using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Founder : StorePermission
    {
        public Founder(Guid userId) : base(userId)
        {

        }

        public override IStorePermission AddAppointment(Guid user, AppointmentType appointment)
        {
            StorePermission prem;
            if (appointment.Equals(AppointmentType.Manager))
            {
                prem = new Manager(user, this);
            }
            else
            {
                prem = new Owner(user, this);
            }
            appointments.TryAdd(user, prem);
            return prem;
        }

        public override void AddPermission(Guid user, Permission permission)
        {
            throw new UnauthorizedAccessException();
        }

        
        public override bool GetPermission(Permission permission)
        {
            return true;
        }

        public override void RemovePermission(Guid user, Permission permission)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
