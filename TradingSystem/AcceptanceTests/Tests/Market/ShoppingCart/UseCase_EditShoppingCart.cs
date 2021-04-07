using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.MarketTests;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 7: edit shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/59
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_EditShoppingCart : MarketMemberTestBase
    {
        private static readonly object[] FixtureArgs =
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
            },
        };

        private UseCase_ViewShoppingCart useCase_viewShoppingCart;

        public ShopImage[] MarketImage => useCase_viewShoppingCart.MarketImage;
        public Func<ShopImage[]> MarketImageFactory { get; }
        public Func<ShopImage[], IEnumerable<ProductInCart>> ChooseProductsForCart { get; }

        public UseCase_EditShoppingCart(SystemContext systemContext, UserInfo userInfo, Func<ShopImage[]> marketImageFactory, Func<ShopImage[], IEnumerable<ProductInCart>> chooseProductsForCart) :
            base(systemContext, userInfo)
        {
            MarketImageFactory = marketImageFactory;
            ChooseProductsForCart = chooseProductsForCart;
        }

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_viewShoppingCart = new UseCase_ViewShoppingCart(
                SystemContext,
                UserInfo,
                MarketImageFactory,
                ChooseProductsForCart
            );
            useCase_viewShoppingCart.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
            useCase_viewShoppingCart.Teardown();
        }

        [TestCase]
        public void Success_AllChanged()
        {
            Assert.IsTrue(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[1].ShopProducts[0].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[0].ShopProducts[0].ProductId, 5) }
            ));
            /// TODO: check cart items changed properly
            /// <see cref="AcceptanceTests.AppInterface.MarketBridge.IMarketBridge.GetShoppingCartItems"/>
        }

        [TestCase]
        public void Failure_NotMutuallyDisjoint()
        {
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[1].ShopProducts[1].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[0].ShopProducts[0].ProductId, 5) }
            ));
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[0].ShopProducts[0].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[0].ShopProducts[0].ProductId, 5) }
            ));
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[1].ShopProducts[0].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 5) }
            ));
        }
    }
}
