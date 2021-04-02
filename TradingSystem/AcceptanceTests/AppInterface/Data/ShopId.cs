using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ShopId
    {
        public ShopId(int id)
        {
            Value = id;
        }

        public int Value { get; }
    }
}
