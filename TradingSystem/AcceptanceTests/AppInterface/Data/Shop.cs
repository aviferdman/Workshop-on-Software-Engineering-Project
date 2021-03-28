using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class Shop : ShopInfo
    {
        public Shop(string name, UserInfo owner, int id) : base(name)
        {
            Owner = owner;
            Id = id;
        }

        public UserInfo Owner { get; }
        public int Id { get; }
    }
}
