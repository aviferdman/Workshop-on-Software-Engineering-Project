using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Manager : StorePermission
    {

        public override StorePermission AddAppointment(Guid user, AppointmentType appointment)
        {
            StorePermission prem;
            if(!this.GetPermission(Permission.AppointManger))
                throw new UnauthorizedAccessException();
            if (appointment.Equals(AppointmentType.Manager))
            {
                prem = new Manager(user, this);

            }
            else
            {
                throw new UnauthorizedAccessException();
            }
            return prem;
        }
        public Manager(Guid userId, StorePermission appoint) : base(userId)
        {
            Appointer = appoint;
            Store_permission.Add(Permission.GetPersonnelInfo);
        }

        public override void AddPermission(Guid user, Permission permission)
        {
            if (Permission.CloseShop.Equals(permission)) //only founder can close shop
                throw new UnauthorizedAccessException();
            if (!Appointer.UserId.Equals(user))
                throw new UnauthorizedAccessException();
            if (!Store_permission.Contains(permission))
            {
                Store_permission.Add(permission);
            }

        }


        public override void RemovePermission(Guid user, Permission permission)
        {
            if (!Appointer.UserId.Equals(user))
                throw new UnauthorizedAccessException();
            if (Store_permission.Contains(permission))
            {
                Store_permission.Remove(permission);
            }

        }
    }
}
