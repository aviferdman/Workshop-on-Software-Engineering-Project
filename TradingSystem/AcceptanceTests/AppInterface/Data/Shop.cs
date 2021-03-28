using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class Shop : ShopInfo
    {
        public Shop(string name) : this(name, -1) { }
        public Shop(string name, int id) : base(name)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
