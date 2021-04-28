
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
    /// Use case 9: change quantity of product in shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/69
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_EditProductInCart : MarketMemberTestBase
    {
        static readonly object[] FixtureArgs =
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
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "design of everyday things",
                            quantity: 90,
                            price: 30,
                            category: "books",
                            weight: 18
                        )),
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "numerical analysis",
                            quantity: 21,
                            price: 12,
                            category: "books",
                            weight: 19
                        )),
                    }
                ),
                (Func<ShopImage, IEnumerable<ProductForCart>>)(
                    shopImage => new ProductForCart[]
                    {
                        new ProductForCart(shopImage.ShopProducts[0], 10),
                    }),
                (Func<ShopImage, IEnumerable<ProductCartEditInfo>>)(
                    shopImage => new ProductCartEditInfo[]
                    {
                        new ProductCartEditInfo(shopImage.ShopProducts[0], 20),
                    }),
            },
        };

        public UseCase_EditProductInCart(
            SystemContext systemContext,
            UserInfo buyerUser,
            ShopImage shopImage,
            Func<ShopImage, IEnumerable<ProductForCart>> productsProviderAdd,
            Func<ShopImage, IEnumerable<ProductCartEditInfo>> productsProviderEdit
        ) : base(systemContext, buyerUser)
        {
            ShopImage = shopImage;
            ProductsProviderAdd = productsProviderAdd;
            ProductsEdit = productsProviderEdit(shopImage);
        }

        public ShopImage ShopImage { get; }
        public Func<ShopImage, IEnumerable<ProductForCart>> ProductsProviderAdd { get; }
        public IEnumerable<ProductCartEditInfo> ProductsEdit { get; }

        private UseCase_AddProductToCart useCase_addProductToCart;
        private List<ProductInCart> ProductsAdditionalTeardown { get; set; }

        public override void Setup()
        {
            base.Setup();

            ProductsAdditionalTeardown = new List<ProductInCart>();
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
            _ = UserBridge.AssureLogin(UserInfo);
            foreach (ProductId productId in ProductsAdditionalTeardown.Select(x => x.ProductId))
            {
                _ = MarketBridge.RemoveProductFromUserCart(productId);
            }
            useCase_addProductToCart.Teardown();
        }

        public IEnumerable<ProductInCart> Success_Normal_NoCheckCartItems()
        {
            IDictionary<ProductId, ProductInCart> expected = ProductForCart.ToDictionary_InCart(useCase_addProductToCart.ProductsAdd);
            foreach (ProductCartEditInfo productEdit in ProductsEdit)
            {
                ProductId productId = productEdit.ProductOriginal.ProductId;
                var productEditBare = new ProductInCart(productId, productEdit.NewQuantity);
                Assert.IsTrue(MarketBridge.EditProductInUserCart(productEditBare));
                expected[productId] = productEditBare;
            }
            return expected.Values;
        }

        [TestCase]
        public void Success_Normal_CheckCartItems()
        {
            IEnumerable<ProductInCart> expected = Success_Normal_NoCheckCartItems();
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                expected,
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }

        [TestCase]
        public void Failure_NoShoppingBasket()
        {
            new UseCase_RemoveProductFromCart_TestLogic
            (
                SystemContext,
                new ProductIdentifiable[] { ShopImage.ShopProducts[0] },
                new ProductForCart[] { new ProductForCart(ShopImage.ShopProducts[0], 10) }
            ).Success_Normal_CheckCartItems();
            Assert.IsFalse(EditProduct(new ProductInCart(ShopImage.ShopProducts[1].ProductId, 3)));
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                Enumerable.Empty<ProductInCart>(),
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }

        [TestCase]
        public void Failure_NotInCart()
        {
            Assert.IsFalse(EditProduct(new ProductInCart(ShopImage.ShopProducts[1].ProductId, 3)));
            AssertCartDidntChange();
        }

        [TestCase]
        public void Failure_ProductDoesntExist()
        {
            Assert.IsFalse(EditProduct(new ProductInCart(new ProductId(Guid.NewGuid()), 3)));
            AssertCartDidntChange();
        }

        [TestCase]
        public void Failure_InvalidQuantity()
        {
            Assert.IsFalse(EditProduct(new ProductInCart(useCase_addProductToCart.ProductsAdd.First().ProductIdentifiable.ProductId, -1)));
            Assert.IsFalse(EditProduct(new ProductInCart(useCase_addProductToCart.ProductsAdd.First().ProductIdentifiable.ProductId, 0)));
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

        private bool EditProduct(ProductInCart product)
        {
            ProductsAdditionalTeardown.Add(product);
            return MarketBridge.EditProductInUserCart(product);
        }
    }
}
