using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Manager 
    {
        private Appointer appointer;
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

        public Manager(string username, Appointer appointer) 
        {
            this.username = username;
            this.appointer = appointer;
            store_permission = new LinkedList<Permission>();
            store_permission.Add(Permission.GetPersonnelInfo);
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
