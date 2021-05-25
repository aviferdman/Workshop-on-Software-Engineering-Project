using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Prem> store_permission { get; set; }




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
            store_permission = new List<Prem>();
            store_permission.Add(new Prem(Permission.GetPersonnelInfo.ToString()));
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
            return store_permission.Where(per=>per.p.Equals(permission.ToString())).Any();
        }

        public bool removePermission(Store store)
        {
            bool ret;
            ret = s.managers.Remove(this);
            return ret;
        }

    }
    public class Prem
    {

        public Prem(string v)
        {
            this.p = v;
        }

        public Prem()
        {
        }

        public string p { get; set; }
    }
}
