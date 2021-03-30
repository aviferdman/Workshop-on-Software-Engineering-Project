using System;
using System.Collections.Concurrent;
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
    public class StorePermission
    {
        private Guid _userId;
        private object _lock;
        private static readonly int FOUNDER = 1;
        private Dictionary<Guid, Permission> _store_permission;
        private Dictionary<Guid, int> _store_hierarchy;

        public StorePermission(Guid userId)
        {
            this._userId = userId;
            this._lock = new object();
            this._store_permission = new Dictionary<Guid, Permission>();
            this._store_hierarchy = new Dictionary<Guid, int>();

        }

        public Guid UserId { get => _userId; set => _userId = value; }
        public Dictionary<Guid, Permission> Store_permission { get => _store_permission; set => _store_permission = value; }
        public Dictionary<Guid, int> Store_hierarchy { get => _store_hierarchy; set => _store_hierarchy = value; }

        //Return the degree of hierarchy of the user in this store (1 is the highest degree, 0 means user not in hierarchy)
        public int GetHierarchy(Guid storeId)
        {
            int hierarchy;
            if (!_store_hierarchy.TryGetValue(storeId, out hierarchy))
            {
                return 0;
            }
            return hierarchy;
        }

        public void SetHierarchy(Guid storeId, int hierarchy)
        {
            _store_hierarchy[storeId] = hierarchy;
        }

        public Permission GetPermission(Guid storeId)
        {
            Permission permission;
            if (!_store_permission.TryGetValue(storeId, out permission))
            {
                return Permission.None;
            }
            return permission;
        }

        public void SetPermission(Guid storeId, Permission permission)
        {
            _store_permission[storeId] = permission;
        }

        public void AddFounder(Guid storeId)
        {
            SetHierarchy(storeId, FOUNDER);
            SetPermission(storeId, Permission.Founder);
        }

        public bool GetUserHistory(Guid requestedUserId)
        {
            return requestedUserId.Equals(_userId);
        }

        public bool AddSubject(Guid storeId, Permission permission, StorePermission subjectStorePermission)
        {
            lock (_lock)
            {
                int myHierarchy = GetHierarchy(storeId);
                Permission myPermission = GetPermission(storeId);
                if (myHierarchy == 0 || permission == Permission.Founder || !CompareGreaterEqual(myPermission, permission)) return false;
                subjectStorePermission.SetHierarchy(storeId, myHierarchy + 1);
                subjectStorePermission.SetPermission(storeId, permission);
                return true;
            }
        }

        public bool GetStoreHistory(Guid storeId)
        {
            Permission userPermission = GetPermission(storeId);
            return CompareGreaterEqual(userPermission, Permission.Manager);
        }

        public bool RemoveSubject(Guid storeId, StorePermission subjectStorePermission)
        {
            lock (_lock)
            {
                int myHierarchy = GetHierarchy(storeId);
                Permission myPermission = GetPermission(storeId);
                int subjectHierarchy = subjectStorePermission.GetHierarchy(storeId);
                Permission subjectPermission = subjectStorePermission.GetPermission(storeId);
                bool ableToRemove = myHierarchy == 0 || myHierarchy > subjectHierarchy || subjectPermission == Permission.Founder || !CompareGreaterEqual(myPermission, subjectPermission);
                if (!ableToRemove) return false;
                subjectStorePermission.SetHierarchy(storeId, 0);
                subjectStorePermission.SetPermission(storeId, Permission.None);
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
