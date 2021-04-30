
using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 5: Add product to shopping basket
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_AddProductToCart : MarketMemberTestBase
    {
        private static object[] FixtureArgs =
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
                            name: "speakers",
                            quantity: 90,
                            price: 30,
                            category: "audio",
                            weight: 0.5
                        )),
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "computer mouse",
                            quantity: 120,
                            price: 40,
                            category: "computer peripherals",
                            weight: 0.4
                        )),
                    }
                ),
                (Func<ShopImage, IEnumerable<ProductForCart>>)(
                    shopImage => new ProductForCart[]
                    {
                        new ProductForCart(shopImage.ShopProducts[0], 60),
                    })
            },
        };

        public ShopImage ShopImage { get; }
        public ShopId ShopId => useCase_addProduct.ShopId;
        public IEnumerable<ProductForCart> ProductsAdd { get; }
        private List<ProductId> ProductsTeardown { get; set; }

        public UseCase_AddProductToCart
        (
            SystemContext systemContext,
            UserInfo buyerUser,
            ShopImage shopImage,
            Func<ShopImage, IEnumerable<ProductForCart>> productsProviderAdd
        ) : base(systemContext, buyerUser)
        {
            ShopImage = shopImage;
            ProductsAdd = productsProviderAdd(shopImage);
        }

        private UseCase_AddProductToShop useCase_addProduct;
        private UseCase_AddProductToCart_TestLogic testLogic;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            ProductsTeardown = new List<ProductId>();

            useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopImage);
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_CheckStoreProducts();

            testLogic = new UseCase_AddProductToCart_TestLogic(SystemContext, UserInfo);
            testLogic.Setup();
        }

        [TearDown]
        public override void Teardown()
        {
            testLogic?.Teardown();
            useCase_addProduct?.Teardown();
        }

        [TestCase]
        public void Success_NoBasket()
        {
            IEnumerable<ProductInCart> products = ProductForCart.ToProductInCart(ProductsAdd);
            testLogic.Success_Normal_CheckCartItems(products, products);
        }

        [TestCase]
        public void Success_BasketExists()
        {
            Success_NoBasket();
            IEnumerable<ProductInCart> productsBefore = ProductForCart.ToProductInCart(ProductsAdd);
            IEnumerable<ProductInCart> productsAdd = new ProductInCart[]
            {
                new ProductInCart(ShopImage.ShopProducts[1].ProductId, 14)
            };
            IEnumerable<ProductInCart> products = productsBefore.Concat(productsAdd);
            testLogic.Success_Normal_CheckCartItems(productsAdd, products);
            testLogic.ProductsTeardown.AddRange(productsAdd.Select(x => x.ProductId));
        }

        [TestCase]
        public void Failure_ProductDoesntExist()
        {
            ProductForCart product = ProductsAdd.First();
            Assert.IsFalse(MarketBridge.AddProductToUserCart(new ProductInCart(new ProductId(Guid.NewGuid()), product.CartQuantity)));
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                Enumerable.Empty<ProductInCart>(),
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }

        [TestCase]
        public void Failure_ProductAlreadyInCart()
        {
            Success_NoBasket();
            ProductForCart product = ProductsAdd.First();
            Assert.IsFalse(MarketBridge.AddProductToUserCart(new ProductInCart(product.ProductIdentifiable.ProductId, product.CartQuantity + 1)));
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                new ProductInCart[] { new ProductInCart(product.ProductIdentifiable.ProductId, product.CartQuantity) },
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }
    }
}
