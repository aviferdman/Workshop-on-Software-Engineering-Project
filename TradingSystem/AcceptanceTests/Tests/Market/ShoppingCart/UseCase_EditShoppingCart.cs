using System;
using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
                User_Buyer,
                UseCase_SearchProduct.DefaultMarketImageFactorry,
                (Func<ShopImage[], IEnumerable<ProductInCart>>)
                  delegate(ShopImage[] marketImage) {
                      return new ProductInCart[] {
                          new ProductInCart(marketImage[1].ShopProducts[0].ProductId, 2),
                          new ProductInCart(marketImage[0].ShopProducts[0].ProductId, 3),
                      };
                  },
                (Func<ShopImage[], ProductsForEdit>)
                  delegate(ShopImage[] marketImage)
                  {
                      return new ProductsForEdit
                      {
                          ProductsAdd = new HashSet<ProductInCart> { new ProductInCart(marketImage[1].ShopProducts[1].ProductId, 10) },
                          ProductsRemove = new HashSet<ProductId> { marketImage[1].ShopProducts[0].ProductId },
                          ProductsEdit = new HashSet<ProductInCart> { new ProductInCart(marketImage[0].ShopProducts[0].ProductId, 5) },
                      };
                  },
            },
        };

        public ShopImage[] MarketImage => useCase_viewShoppingCart.MarketImage;
        public Func<ShopImage[]> MarketImageFactory { get; }
        public Func<ShopImage[], IEnumerable<ProductInCart>> ChooseProductsForCart { get; }
        public Func<ShopImage[], ProductsForEdit> ChooseProductsForEdit { get; }

        public UseCase_EditShoppingCart
        (
            SystemContext systemContext,
            UserInfo userInfo,
            Func<ShopImage[]> marketImageFactory,
            Func<ShopImage[], IEnumerable<ProductInCart>> chooseProductsForCart,
            Func<ShopImage[], ProductsForEdit> chooseProductsForEdit
        ) : base(systemContext, userInfo)
        {
            MarketImageFactory = marketImageFactory;
            ChooseProductsForCart = chooseProductsForCart;
            ChooseProductsForEdit = chooseProductsForEdit;
        }

        private ProductsForEdit productsForEdit;

        private UseCase_ViewShoppingCart useCase_viewShoppingCart;

        public override void Setup()
        {
            base.Setup();

            // We actually just want this for the setup
            useCase_viewShoppingCart = new UseCase_ViewShoppingCart
            (
                SystemContext,
                UserInfo,
                MarketImageFactory,
                ChooseProductsForCart
            );
            useCase_viewShoppingCart.Setup();

            productsForEdit = ChooseProductsForEdit(MarketImage);
        }

        public override void Teardown()
        {
            base.Teardown();
            if (productsForEdit != null && productsForEdit.ProductsAdd != null)
            {
                foreach (ProductInCart product in productsForEdit.ProductsAdd)
                {
                    MarketBridge.RemoveProductFromUserCart(product.ProductId);
                }
            }
            useCase_viewShoppingCart.Teardown();
        }

        [TestCase]
        public void Success_AllChanged()
        {
            Assert.IsTrue(MarketBridge.EditUserCart(
                productsForEdit.ProductsAdd,
                productsForEdit.ProductsRemove,
                productsForEdit.ProductsEdit
            ));
            /// TODO: check cart items changed properly
            /// <see cref="AcceptanceTests.AppInterface.MarketBridge.IMarketBridge.GetShoppingCartItems"/>
        }

        [TestCase]
        public void Failure_NotMutuallyDisjoint()
        {
            Assert.IsFalse(MarketBridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[1].ShopProducts[1].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[0].ShopProducts[0].ProductId, 5) }
            ));
            Assert.IsFalse(MarketBridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[0].ShopProducts[0].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[0].ShopProducts[0].ProductId, 5) }
            ));
            Assert.IsFalse(MarketBridge.EditUserCart(
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 10) },
                new HashSet<ProductId> { MarketImage[1].ShopProducts[0].ProductId },
                new HashSet<ProductInCart> { new ProductInCart(MarketImage[1].ShopProducts[1].ProductId, 5) }
            ));
        }
    }
}
