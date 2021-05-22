using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Manager 
    {
        public MemberState appointer { get; set; }
        public MemberState m { get; set; }
        public Store s { get; set; }
        public string username { get; set; }
        public Guid sid { get; set; }
        public ICollection<string> store_permission { get; set; }

        public string Username { get => username; set => username = value; }
        public ICollection<string> Store_permission { get => store_permission; set => store_permission = value; }
        public MemberState M { get => m; set => m = value; }
        public Store S { get => s; set => s = value; }


        public enum Permission
        {
            AddProduct,
            AppointManger,
            RemoveProduct,
            GetPersonnelInfo,
            EditProduct,
            GetShopHistory,
            EditPermissions,
            CloseShop,
            EditDiscount,
            EditPolicy,
            BidRequests
        }

        public Manager() {
            username = "manager";
        }

        private Manager(MemberState m, Store s, MemberState appointer) 
        {
            this.username = m.UserId;
            this.m = m;
            this.s = s;
            this.sid = sid;
            this.appointer = appointer;
            store_permission = new LinkedList<string>();
            store_permission.Add(Permission.GetPersonnelInfo.ToString());
        }
        public static Manager makeManager(MemberState m, Store s, Appointer appointer)
        {
            if ( s.isStaff(m.UserId))
                throw new InvalidOperationException();
            Manager man = new Manager(m, s, appointer.getM());
            s.Managers.Add(man);
            return man;
        }

        public virtual bool GetPermission(Permission permission)
        {
            return store_permission.Contains(permission.ToString());
        }

        public bool removePermission(Store store)
        {
            bool ret;
            lock (m.managerPrems)
            {
                ret = m.managerPrems.Remove(this)||s.managers.Remove(this)|| appointer.managerAppointments.Remove(this);
            }
            return ret;
        }

    }
}
