using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Manager : IManager
    {
        private Appointer appointer;
        private MemberState m;
        private Store s;
        private string username;
        private ICollection<Permission> store_permission;

        public string Username { get => username; set => username = value; }
        public ICollection<Permission> Store_permission { get => store_permission; set => store_permission = value; }
        public MemberState M { get => m; set => m = value; }

        public enum Permission
        {
            AddProduct,
            AppointManger,
            RemoveProduct,
            GetPersonnelInfo,
            EditProduct,
            GetShopHistory,
            EditPermissions,
            CloseShop
        }

        private Manager(MemberState m, Store s, Appointer appointer) 
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
            this.appointer = appointer;
            store_permission = new LinkedList<Permission>();
            store_permission.Add(Permission.GetPersonnelInfo);
        }
        public static Manager makeManager(MemberState m, Store s, Appointer appointer)
        {
            if (m.isStaff(s) || s.isStaff(m.UserId))
                throw new InvalidOperationException();
            Manager man = new Manager(m, s, appointer);
            m.ManagerPrems.TryAdd(s, man);
            s.Managers.TryAdd(m.UserId, man);
            return man;
        }

        public bool GetPermission(Permission permission)
        {
            return store_permission.Contains(permission);
        }

        public bool removePermission(Store store)
        {
            bool ret;
            lock (m.Prem_lock)
            {
                ret = m.ManagerPrems.TryRemove(store, out _);
            }
            return ret;
        }
    }
}
