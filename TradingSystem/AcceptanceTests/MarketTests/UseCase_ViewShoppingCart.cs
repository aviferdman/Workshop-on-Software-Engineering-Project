using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
        static object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
            },
        };

        public UseCase_ViewShoppingCart(SystemContext systemContext, UserInfo userInfo)
            : base(systemContext, userInfo)
        { }

        private UseCase_SearchProduct useCase_search;
        private UseCase_AddProductToCart_TestLogic addToCart_logic;

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_search = new UseCase_SearchProduct(SystemContext, UserInfo);
            useCase_search.Setup();

            addToCart_logic = new UseCase_AddProductToCart_TestLogic(SystemContext);
            // already setup earlier
            addToCart_logic.Success_Normal(useCase_search.Product2_1);
            addToCart_logic.Success_Normal(useCase_search.Product1_1);
        }

        public override void Teardown()
        {
            base.Teardown();

            // TODO:
            // 1. Remove items from cart
            // 2. Teardown search
        }

        [TestCase]
        public void Success_Normal()
        {
            IEnumerable<ProductId>? cartItems = Bridge.GetShoppingCartItems();
            new Assert_SetEquals<ProductId>("View shopping cart - success", useCase_search.Product2_1, useCase_search.Product1_1)
                .AssertEquals(cartItems);
        }
    }
}
