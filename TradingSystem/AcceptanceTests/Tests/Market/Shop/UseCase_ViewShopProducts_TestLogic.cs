using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop
{
    public class UseCase_ViewShopProducts_TestLogic : MarketTestBase
    {
        public UseCase_ViewShopProducts_TestLogic(SystemContext systemContext)
            : base(systemContext)
        {

        }

        public void Success_Normal(string testName, ShopId shopId, IEnumerable<ProductIdentifiable> expected)
        {
            IEnumerable<ProductIdentifiable>? resultProducts = Bridge.GetShopProducts(shopId);
            Assert.IsNotNull(resultProducts);
            Assert.IsTrue(resultProducts.All(x => x.ProductId.IsValid()), $"{testName}: contains invalid product IDs");
            new Assert_SetEquals<ProductIdentifiable>
            (
                testName,
                expected,
                ProductIdentifiable.DeepEquals
            ).AssertEquals(resultProducts);
        }
    }
}
