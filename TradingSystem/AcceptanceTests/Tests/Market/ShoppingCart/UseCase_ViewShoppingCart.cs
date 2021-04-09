using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
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
                UseCase_SearchProduct.DefaultMarketImageFactorry,
                (Func<ShopImage[], IEnumerable<ProductInCart>>)
                  delegate(ShopImage[] marketImage) {
                      return new ProductInCart[] {
                          new ProductInCart(marketImage[1].ShopProducts[0].ProductId, 2),
                          new ProductInCart(marketImage[0].ShopProducts[0].ProductId, 3),
                      };
                  },
            }
        };

        private readonly Func<ShopImage[], IEnumerable<ProductInCart>> chooseProductsForCart;

        public UseCase_ViewShoppingCart(
            SystemContext systemContext,
            UserInfo userInfo,
            Func<ShopImage[]> marketImageFactory,
            Func<ShopImage[], IEnumerable<ProductInCart>> chooseProductsForCart
        )
            : base(systemContext, userInfo)
        {
            MarketImageFactory = marketImageFactory;
            this.chooseProductsForCart = chooseProductsForCart;
        }

        private UseCase_SearchProduct useCase_search;
        private UseCase_AddProductToCart_TestLogic addToCart_logic;

        public Func<ShopImage[]> MarketImageFactory { get; }
        public IEnumerable<ProductInCart> ChosenProducts { get; private set; }

        public ShopImage[] MarketImage => useCase_search.MarketImage;

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_search = new UseCase_SearchProduct(SystemContext, UserInfo, MarketImageFactory);
            useCase_search.Setup();
            ChosenProducts = chooseProductsForCart(MarketImage);

            // already setup earlier
            addToCart_logic = new UseCase_AddProductToCart_TestLogic(SystemContext);
            foreach (ProductInCart product in ChosenProducts)
            {
                addToCart_logic.Success_Normal(product);
            }
        }

        public override void Teardown()
        {
            base.Teardown();

            if (ChosenProducts != null)
            {
                foreach (ProductInCart product in ChosenProducts)
                {
                    _ = Bridge.RemoveProductFromUserCart(product.ProductId);
                }
            }
            useCase_search.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            IEnumerable<ProductInCart>? cartItems = Bridge.GetShoppingCartItems();
            new Assert_SetEquals<ProductId, ProductInCart>(
                "View shopping cart - success",
                ChosenProducts,
                x => x.ProductId
            ).AssertEquals(cartItems);
        }
    }
}
