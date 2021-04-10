using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Manager : StorePermission
    {

        public override IStorePermission AddAppointment(Guid user, AppointmentType appointment)
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
            appointments.TryAdd(user, prem);
            return prem;
        }
        public Manager(Guid userId, IStorePermission appoint) : base(userId)
        {
            appointer = (StorePermission)appoint;
            store_permission.Add(Permission.GetPersonnelInfo);
        }

        public override void AddPermission(Guid user, Permission permission)
        {
            if (Permission.CloseShop.Equals(permission)) //only founder can close shop
                throw new UnauthorizedAccessException();
            if (!appointer.UserId.Equals(user))
                throw new UnauthorizedAccessException();
            if (!store_permission.Contains(permission))
            {
                store_permission.Add(permission);
            }

        }


        public override void RemovePermission(Guid user, Permission permission)
        {
            if (!appointer.UserId.Equals(user))
                throw new UnauthorizedAccessException();
            if (!store_permission.Contains(permission))
            {
                store_permission.Add(permission);
            }

        }
    }
}
