using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 8: remove product from shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/68
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_RemoveProductFromCart : MarketMemberTestBase
    {
        static object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User_Buyer,
                new ShopImage
                (
                    User_ShopOwner1,
                    Shop1,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo(
                            name: "modern operating system",
                            quantity: 90,
                            price: 30,
                            category: "books",
                            weight: 15
                        )),
                        new ProductIdentifiable(new ProductInfo(
                            name: "modern warfare 2",
                            quantity: 500,
                            price: 35,
                            category: "video games",
                            weight: 0.3
                        )),
                    }
                ),
                (Func<ShopImage, IEnumerable<ProductForCart>>)(
                    shopImage => new ProductForCart[]
                    {
                        new ProductForCart(shopImage.ShopProducts[0], 30),
                        new ProductForCart(shopImage.ShopProducts[1], 5),
                    }),
                (Func<ShopImage, IEnumerable<ProductIdentifiable>>)(
                    shopImage => new ProductIdentifiable[]
                    {
                        shopImage.ShopProducts[1],
                    })
            },
        };

        public UseCase_RemoveProductFromCart
        (
            SystemContext systemContext,
            UserInfo buyerUser,
            ShopImage shopImage,
            Func<ShopImage, IEnumerable<ProductForCart>> productsProviderAdd,
            Func<ShopImage, IEnumerable<ProductIdentifiable>> productsProviderRemove
        ) : base(systemContext, buyerUser)
        {
            ShopImage = shopImage;
            ProductsProviderAdd = productsProviderAdd;
            ProductsRemove = productsProviderRemove(shopImage);
        }

        public ShopImage ShopImage { get; }
        public Func<ShopImage, IEnumerable<ProductForCart>> ProductsProviderAdd { get; }
        public IEnumerable<ProductIdentifiable> ProductsRemove { get; }


        private UseCase_AddProductToCart useCase_addProductToCart;

        public override void Setup()
        {
            base.Setup();

            useCase_addProductToCart = new UseCase_AddProductToCart
            (
                SystemContext,
                UserInfo,
                ShopImage,
                ProductsProviderAdd
            );
            useCase_addProductToCart.Setup();
            useCase_addProductToCart.Success_NoBasket();
        }

        public override void Teardown()
        {
            base.Teardown();
            useCase_addProductToCart.Teardown();
        }

        public void Success_Normal_NoCheckCartItems()
        {
            foreach (ProductIdentifiable product in ProductsRemove)
            {
                Assert.IsTrue(Bridge.RemoveProductFromUserCart(product.ProductId));
            }
        }

        [TestCase]
        public void Success_Normal_CheckCartItems()
        {
            Success_Normal_NoCheckCartItems();
            CheckCartItems();
        }

        private void CheckCartItems()
        {
            new Assert_SetEquals<ProductInCart>(CalculateExpected())
                .AssertEquals(Bridge.GetShoppingCartItems());
        }

        private IEnumerable<ProductInCart> CalculateExpected()
        {
            IDictionary<ProductId, ProductForCart> expected = ProductForCart.ToDictionary(useCase_addProductToCart.ProductsAdd);
            foreach (ProductIdentifiable productRemoved in ProductsRemove)
            {
                _ = expected.Remove(productRemoved.ProductId);
            }
            return ProductForCart.ToProductInCart(expected.Values);
        }
    }
}
