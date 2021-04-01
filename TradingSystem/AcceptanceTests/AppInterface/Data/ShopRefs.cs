using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ShopRefs
    {
        public ShopRefs(ShopId shopId, UserInfo owner)
        {
            Id = shopId;
            Owner = owner;
        }

        public ShopId Id { get; }
        public UserInfo Owner { get; }
    }
}
