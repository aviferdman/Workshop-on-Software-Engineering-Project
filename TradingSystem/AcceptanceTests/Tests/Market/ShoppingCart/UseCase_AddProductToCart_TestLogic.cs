
using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public class UseCase_AddProductToCart_TestLogic : MarketTestBase
    {
        public UseCase_AddProductToCart_TestLogic(SystemContext systemContext) :
            base(systemContext)
        { }

        public void Success_Normal_NoCheckCartItems(IEnumerable<ProductInCart> productsAdd)
        {
            foreach (ProductInCart product in productsAdd)
            {
                Assert.IsTrue(MarketBridge.AddProductToUserCart(product));
            }
        }

        public void Success_Normal_CheckCartItems(IEnumerable<ProductInCart> productsAdd, IEnumerable<ProductInCart> expected)
        {
            Success_Normal_NoCheckCartItems(productsAdd);
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                expected,
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }
    }
}
