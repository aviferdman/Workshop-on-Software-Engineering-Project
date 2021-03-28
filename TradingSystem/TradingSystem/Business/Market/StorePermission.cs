using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public enum Permission
    {
        Founder,
        Owner,
        Manager,
        None
    }
    class StorePermission
    {
        private Guid _userId;
        private object _lock;
        private static readonly int FOUNDER = 1;

        public StorePermission(Guid userId)
        {
            this._userId = userId;
            this._lock = new object();
        }

        public Guid UserId { get => _userId; set => _userId = value; }
        
        //Return the degree of hierarchy of the user in this store (1 is the highest degree, 0 means user not in hierarchy)
        public int GetHierarchy(Guid userId, Guid storeId)
        {
            //Get from DB
            throw new NotImplementedException();
        }

        public void SetHierarchy(Guid userId, Guid storeId, int hierarchy)
        {
            //Update DB
            throw new NotImplementedException();
        }

        public Permission GetPermission(Guid userId, Guid storeId)
        {
            //Get from DB
            throw new NotImplementedException();
        }

        public void SetPermission(Guid userId, Guid storeId, Permission permission)
        {
            //Update DB
            throw new NotImplementedException();
        }

        public void AddFounder(Guid userId, Guid storeId)
        {
            SetHierarchy(userId, storeId, FOUNDER);
            SetPermission(userId, storeId, Permission.Founder);
        }

        public bool GetUserHistory(Guid requestedUserId)
        {
            return requestedUserId.Equals(_userId);
        }

        public bool AddSubject(Guid newManagerId, Guid storeId, Permission permission)
        {
            lock (_lock)
            {
                int myHierarchy = GetHierarchy(_userId, storeId);
                Permission myPermission = GetPermission(_userId, storeId);
                if (myHierarchy == 0 || permission == Permission.Founder || !CompareGreaterEqual(myPermission, permission)) return false;
                SetHierarchy(newManagerId, storeId, myHierarchy + 1);
                SetPermission(newManagerId, storeId, permission);
                return true;
            }
        }

        public bool GetStoreHistory(Guid userId, Guid storeId)
        {
            Permission userPermission = GetPermission(userId, storeId);
            return CompareGreaterEqual(userPermission, Permission.Manager);
        }

        public bool RemoveSubject(Guid managerId, Guid storeId)
        {
            lock (_lock)
            {
                int myHierarchy = GetHierarchy(_userId, storeId);
                Permission myPermission = GetPermission(_userId, storeId);
                int removedManagerHierarchy = GetHierarchy(managerId, storeId);
                Permission removedManagerPermission = GetPermission(managerId, storeId);
                bool ableToRemove = myHierarchy == 0 || myHierarchy > removedManagerHierarchy || removedManagerPermission == Permission.Founder || !CompareGreaterEqual(myPermission, removedManagerPermission);
                if (!ableToRemove) return false;
                SetHierarchy(managerId, storeId, 0);
                SetPermission(managerId, storeId, Permission.None);
                return true;
            }
        }

        private bool CompareGreaterEqual(Permission myPermission, Permission permission)
        {
            switch (myPermission)
            {
                case Permission.Founder:
                    return true;
                case Permission.Owner:
                    return permission == Permission.Owner || permission == Permission.Manager || permission == Permission.None;
                case Permission.Manager:
                    return permission == Permission.Manager || permission == Permission.None;
                default:
                    return false;
            }       
        }
    }
}
