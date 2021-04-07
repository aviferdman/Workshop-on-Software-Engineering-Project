using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market
{
    public class ShopImage
    {
        public ShopImage(UserInfo ownerUser, ShopInfo shopInfo, ProductIdentifiable[] shopProducts)
        {
            OwnerUser = ownerUser;
            ShopInfo = shopInfo;
            ShopProducts = shopProducts;
        }

        public UserInfo OwnerUser { get; }
        public ShopInfo ShopInfo { get; }
        public ProductIdentifiable[] ShopProducts { get; }
    }
}
