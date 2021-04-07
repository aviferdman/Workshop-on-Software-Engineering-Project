using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Owner : StorePermission
    {
        public override StorePermission AddAppointment(Guid user, AppointmentType appointment)
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
        public Owner(Guid userId, StorePermission appoint) : base(userId)
        {
            base.appointer = appoint;
        }
        
        public override void AddPermission(Guid user, Permission permission)
        {
            throw new UnauthorizedAccessException();
        }


        public override bool GetPermission(Permission permission)
        {
            if (Permission.CloseShop.Equals(permission))//only founder can close shop
                return false;
            return true;
        }

        public override void RemovePermission(Guid user, Permission permission)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
