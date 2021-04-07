using System;
using System.Collections.Generic;
using System.Linq;
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
                (Func<IList<IList<ProductId>>, IEnumerable<ProductInCart>>)
                  delegate(IList<IList<ProductId>> products) {
                      return new ProductInCart[] {
                          new ProductInCart(products[1][0], 2),
                          new ProductInCart(products[0][0], 3),
                      };
                  },
            }
        };

        private readonly Func<IList<IList<ProductId>>, IEnumerable<ProductInCart>> chooseProductsForCart;

        public UseCase_ViewShoppingCart(
            SystemContext systemContext,
            UserInfo userInfo,
            ShopImage[] marketImage,
            Func<IList<IList<ProductId>>, IEnumerable<ProductInCart>> chooseProductsForCart
        )
            : base(systemContext, userInfo)
        {
            MarketImage = marketImage;
            this.chooseProductsForCart = chooseProductsForCart;
        }

        private UseCase_SearchProduct useCase_search;
        private UseCase_AddProductToCart_TestLogic addToCart_logic;

        public ShopImage[] MarketImage { get; }
        public IList<IList<ProductId>> Products { get; private set; }
        public IEnumerable<ProductInCart> ChosenProducts { get; private set; }

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_search = new UseCase_SearchProduct(SystemContext, UserInfo, MarketImage);
            useCase_search.Setup();
            Products = useCase_search.Products;
            ChosenProducts = chooseProductsForCart(Products);

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
            new Assert_SetEquals<ProductInCart>("View shopping cart - success", ChosenProducts)
                .AssertEquals(cartItems);
        }
    }
}
