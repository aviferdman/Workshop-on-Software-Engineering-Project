using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ShopInfo
    {
        public ShopInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
