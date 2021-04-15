using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Manager 
    {
        private Appointer appointer;
        private MemberState m;
        private Store s;
        private string username;
        private ICollection<Permission> store_permission;

        public string Username { get => username; set => username = value; }

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
            return new Manager(m, s, appointer);
        }

        public bool GetPermission(Permission permission)
        {
            return store_permission.Contains(permission);
        }
        //don't!!!  user this method it is called from the appointer after checking he is the appointer
        public  void AddPermission(Permission permission)
        {
            if (Permission.CloseShop.Equals(permission)) //only founder can close shop
                throw new UnauthorizedAccessException();
            if (!store_permission.Contains(permission))
            {
                store_permission.Add(permission);
            }

        }

        //don't!!!  user this method it is called from the appointer after checking he is the appointer
        public void RemovePermission (Permission permission)
        {
            if (!store_permission.Contains(permission))
            {
                store_permission.Add(permission);
            }

        }
    }
}
