
using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    /// <summary>
    /// Acceptance test for
    /// Use case 24: remove product from shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/19
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_RemoveProductFromShop : ShopManagementTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                new ShopImage
                (
                    User_ShopOwner1,
                    Shop1,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo(
                            name: "cucumber",
                            quantity: 7,
                            price: 4,
                            category: "vegetable",
                            weight: 0.45
                        ))
                    }
                ),
                (Func<ShopImage, IEnumerable<ProductIdentifiable>>)
                    (shopImage => shopImage.ShopProducts)
            },
        };

        public UseCase_RemoveProductFromShop
        (
            SystemContext systemContext,
            ShopImage shopImage,
            Func<ShopImage, IEnumerable<ProductIdentifiable>> productsProviderRemove
        ) : base(systemContext, shopImage.OwnerUser)
        {
            ShopImage = shopImage;
            ProductsProviderRemove = productsProviderRemove;
        }

        public ShopImage ShopImage { get; }
        public Func<ShopImage, IEnumerable<ProductIdentifiable>> ProductsProviderRemove { get; }

        public ShopId ShopId => useCase_addProduct.ShopId;

        private UseCase_AddProductToShop useCase_addProduct;
        private IEnumerable<ProductIdentifiable> productsToRemove;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopImage);
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_CheckStoreProducts();
            productsToRemove = ProductsProviderRemove(ShopImage);
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_addProduct.Teardown();
        }

        [TestCase]
        public void Success_CheckStoreProducts()
        {
            IEnumerable<ProductIdentifiable> expected = Success_NoCheckStoreProducts();
            new UseCase_ViewShopProducts_TestLogic(SystemContext)
                .Success_Normal(ShopId, expected);
        }

        public IEnumerable<ProductIdentifiable> Success_NoCheckStoreProducts()
        {
            RemoveProducts();
            return CalculateExpected();
        }

        private void RemoveProducts()
        {
            foreach (ProductIdentifiable product in productsToRemove)
            {
                Assert.IsTrue(MarketBridge.RemoveProductFromShop(ShopId, product.ProductId));
            }
        }

        private IEnumerable<ProductIdentifiable> CalculateExpected()
        {
            ISet<ProductIdentifiable> expected = new HashSet<ProductIdentifiable>(ShopImage.ShopProducts);
            expected.ExceptWith(productsToRemove);
            return expected;
        }

        public void Failure_NotLoggedIn()
        {
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            ProductId productId = productsToRemove.First().ProductId;
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(ShopId, productId));
        }

        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            ProductId productId = productsToRemove.First().ProductId;
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(ShopId, productId));
        }

        public void Failure_ShopDoesntExist()
        {
            ProductId productId = productsToRemove.First().ProductId;
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(new ShopId(Guid.NewGuid(), "notexists"), productId));
        }

        public void Failure_InvalidShopId()
        {
            ProductId productId = productsToRemove.First().ProductId;
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(default, productId));
        }

        [TestCase]
        public void Failure_InvalidProductId()
        {
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(ShopId, default));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            Assert.IsFalse(MarketBridge.RemoveProductFromShop(ShopId, new ProductId(Guid.NewGuid())));
        }

        [TestCase]
        public void Failure_ProductNotInShop()
        {
            var product2 = new ProductIdentifiable(new ProductInfo
            (
                name: "tv remote control",
                quantity: 300,
                price: 7,
                category: "tv peripherals",
                weight: 0.8
            ));
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            useCase_addProduct = new UseCase_AddProductToShop
            (
                SystemContext,
                new ShopImage(UserInfo, Shop2, new ProductIdentifiable[] { product2 })
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_CheckStoreProducts();

            Assert.IsFalse(MarketBridge.RemoveProductFromShop(ShopId, product2.ProductId));
        }
    }
}
