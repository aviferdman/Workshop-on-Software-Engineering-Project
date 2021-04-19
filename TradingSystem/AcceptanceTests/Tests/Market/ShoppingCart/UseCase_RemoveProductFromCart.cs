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
                    }),
                (Func<ShopImage, IEnumerable<ProductIdentifiable>>)(
                    shopImage => new ProductIdentifiable[]
                    {
                        shopImage.ShopProducts[0],
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
        private UseCase_RemoveProductFromCart_TestLogic testLogic;

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
            testLogic = new UseCase_RemoveProductFromCart_TestLogic
            (
                SystemContext,
                ProductsRemove,
                useCase_addProductToCart.ProductsAdd
            );
        }

        public override void Teardown()
        {
            base.Teardown();
            useCase_addProductToCart.Teardown();
        }

        public void Success_Normal_NoCheckCartItems()
        {
            testLogic.Success_Normal_NoCheckCartItems();
        }

        [TestCase]
        public void Success_Normal_CheckCartItems()
        {
            testLogic.Success_Normal_CheckCartItems();
        }

        [TestCase]
        public void Failure_NoShoppingBasket()
        {
            Success_Normal_CheckCartItems();
            Assert.IsFalse(MarketBridge.RemoveProductFromUserCart(ShopImage.ShopProducts[1].ProductId));
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                Enumerable.Empty<ProductInCart>(),
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }

        [TestCase]
        public void Failure_NotInCart()
        {
            Assert.IsFalse(MarketBridge.RemoveProductFromUserCart(ShopImage.ShopProducts[1].ProductId));
            AssertCartDidntChange();
        }

        [TestCase]
        public void Failure_ProductDoesntExist()
        {
            Assert.IsFalse(MarketBridge.RemoveProductFromUserCart(new ProductId(Guid.NewGuid())));
            AssertCartDidntChange();
        }

        private void AssertCartDidntChange()
        {
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                ProductForCart.ToProductInCart(useCase_addProductToCart.ProductsAdd),
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }
    }
}
