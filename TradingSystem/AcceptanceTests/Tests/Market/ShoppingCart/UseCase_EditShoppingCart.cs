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
                UseCase_SearchProduct.DefaultMarketImage,
            },
        };

        private UseCase_ViewShoppingCart useCase_viewShoppingCart;

        public ShopImage[] MarketImage { get; }
        public IList<IList<ProductId>> Products { get; private set; }

        public UseCase_EditShoppingCart(SystemContext systemContext, UserInfo userInfo, ShopImage[] marketImage) :
            base(systemContext, userInfo)
        {
            MarketImage = marketImage;
        }

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_viewShoppingCart = new UseCase_ViewShoppingCart(
                SystemContext,
                UserInfo,
                MarketImage,
                products => new ProductInCart[] {
                    new ProductInCart(products[1][0], 6),
                    new ProductInCart(products[0][0], 7),
                }
            );
            useCase_viewShoppingCart.Setup();
            Products = useCase_viewShoppingCart.Products;
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
                new HashSet<ProductInCart> { new ProductInCart(Products[1][1], 10) },
                new HashSet<ProductId> { Products[1][0] },
                new HashSet<ProductInCart> { new ProductInCart(Products[0][0], 5) }
            ));
            /// TODO: check cart items changed properly
            /// <see cref="AcceptanceTests.AppInterface.MarketBridge.IMarketBridge.GetShoppingCartItems"/>
        }

        [TestCase]
        public void Failure_NotMutuallyDisjoint()
        {
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(Products[1][1], 10) },
                new HashSet<ProductId> { Products[1][1] },
                new HashSet<ProductInCart> { new ProductInCart(Products[0][0], 5) }
            ));
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(Products[1][1], 10) },
                new HashSet<ProductId> { Products[0][0] },
                new HashSet<ProductInCart> { new ProductInCart(Products[0][0], 5) }
            ));
            Assert.IsFalse(Bridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(Products[1][1], 10) },
                new HashSet<ProductId> { Products[1][0] },
                new HashSet<ProductInCart> { new ProductInCart(Products[1][1], 5) }
            ));
        }
    }
}
