using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 6: View shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/54
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_ViewShoppingCart : MarketMemberTestBase
    {
        static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
                UseCase_SearchProduct.DefaultMarketImage,
            },
        };

        public UseCase_ViewShoppingCart(SystemContext systemContext, UserInfo userInfo, ShopImage[] marketImage)
            : base(systemContext, userInfo)
        {
            MarketImage = marketImage;
        }

        private UseCase_SearchProduct useCase_search;
        private UseCase_AddProductToCart_TestLogic addToCart_logic;

        public ShopImage[] MarketImage { get; }

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_search = new UseCase_SearchProduct(SystemContext, UserInfo, MarketImage);
            useCase_search.Setup();

            addToCart_logic = new UseCase_AddProductToCart_TestLogic(SystemContext);
            // already setup earlier
            addToCart_logic.Success_Normal(useCase_search.Products[1][0]);
            addToCart_logic.Success_Normal(useCase_search.Products[0][0]);
        }

        public override void Teardown()
        {
            base.Teardown();

            _ = Bridge.RemoveProductFromUserCart(useCase_search.Products[1][0]);
            _ = Bridge.RemoveProductFromUserCart(useCase_search.Products[0][0]);
            useCase_search.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            IEnumerable<ProductId>? cartItems = Bridge.GetShoppingCartItems();
            new Assert_SetEquals<ProductId>("View shopping cart - success", useCase_search.Products[1][0], useCase_search.Products[0][0])
                .AssertEquals(cartItems);
        }
    }
}
