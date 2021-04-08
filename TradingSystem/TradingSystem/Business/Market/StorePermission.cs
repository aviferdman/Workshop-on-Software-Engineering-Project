﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    //all possible actions a manger can take, add more as needed
    public enum Permission
    {
        AddProduct,
        AppointManger,
        RemoveProduct,
        GetPersonnelInfo,
        EditProduct,
        GetShopHistory,

        CloseShop
    }

    public enum AppointmentType
    {
        Owner,
        Manager
    }
    public abstract class StorePermission : IStorePermission
    {
        private Guid userId;
        protected object _lock;
        protected ICollection<Permission> store_permission; //users the user appointed
        protected ConcurrentDictionary<Guid, StorePermission> appointments;
        protected StorePermission appointer;//the user that appointed the user, null for founder

        public StorePermission(Guid userId)
        {
            this.userId = userId;
            this._lock = new object();
            this.Store_permission = new LinkedList<Permission>();
            this.Appointments = new ConcurrentDictionary<Guid, StorePermission>();
            this.Appointer = null;
        }

        public Guid UserId { get => userId; set => userId = value; }
        public ICollection<Permission> Store_permission { get => store_permission; set => store_permission = value; }
        protected ConcurrentDictionary<Guid, StorePermission> Appointments { get => appointments; set => appointments = value; }
        protected StorePermission Appointer { get => appointer; set => appointer = value; }

        //Return the degree of hierarchy of the user in this store (1 is the highest degree, 0 means user not in hierarchy)

        public virtual bool GetPermission(Permission permission)
        {
            return Store_permission.Contains(permission);
        }

        //add premmition only appointer can add a premmition to manager
        public abstract void AddPermission(Guid user, Permission permission);
       

        //remove premmition only appointer can remove a premmition from manager
        public abstract void RemovePermission(Guid user, Permission permission);

        /// checks if this user can appoint a user and if he does crates a new StorePremmition according to appointment 
        /// and adds him to <see cref="Appointments"/>
        /// returns the new StorePremmition or null if user can't be appointed
        public abstract StorePermission AddAppointment(Guid user, AppointmentType appointment );

        /// checks if this user can remove the appiontment of a user and if he does removes him from <see cref="Appointments"/>
        public bool canRemoveAppointment( Guid userToRemove)
        {
            StorePermission s;
            return Appointments.TryRemove(userToRemove, out s);
        }











    }
}
