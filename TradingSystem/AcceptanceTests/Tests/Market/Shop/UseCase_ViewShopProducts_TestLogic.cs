using System.Collections.Generic;
using System.Linq;

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

        public void Success_Normal(ShopId shopId, IEnumerable<ProductIdentifiable> expected)
        {
            IEnumerable<ProductIdentifiable>? resultProducts = MarketBridge.GetShopProducts(shopId);
            Assert.IsNotNull(resultProducts);
            Assert.IsTrue(resultProducts.All(x => x.ProductId.IsValid()), $"contains invalid product IDs");
            new Assert_SetEquals<ProductIdentifiable>
            (
                expected,
                ProductIdentifiable.DeepEquals
            ).AssertEquals(resultProducts);
        }
    }
}
