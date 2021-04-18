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
    /// Use case 25: edit product in shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/20
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_EditShopProductInShop : ShopManagementTestBase
    {
        private static object[] FixtureArgs = new object[]
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
                            name: "WiiU",
                            quantity: 80,
                            price: 1000,
                            category: "gaming consoles",
                            weight: 5
                        )),
                        new ProductIdentifiable(new ProductInfo(
                            name: "garbage can",
                            quantity: 100,
                            price: 20,
                            category: "garbage cans",
                            weight: 1
                        )),
                    }
                ),
                (Func<ShopImage, IEnumerable<ProductShopEditInfo>>)(
                    shopImage =>
                    new ProductShopEditInfo[]
                    {
                        new ProductShopEditInfo(
                            shopImage.ShopProducts[0],
                            new ProductInfo(
                                name: "WiiU",
                                quantity: 90,
                                price: 1100,
                                category: "gaming consoles",
                                weight: 5
                            )
                        ),
                        new ProductShopEditInfo(
                            shopImage.ShopProducts[1],
                            new ProductInfo(
                                name: "garbage can not haHAA",
                                quantity: 80,
                                price: 15,
                                category: "garbage cans",
                                weight: 1
                            )
                        ),
                    }
                ),
            },
        };

        private UseCase_AddProductToShop useCase_addProduct;

        public UseCase_EditShopProductInShop
        (
            SystemContext systemContext,
            ShopImage shopImage,
            Func<ShopImage, IEnumerable<ProductShopEditInfo>> productsProvidersEdit
        ) :
            base(systemContext, shopImage.OwnerUser)
        {
            ShopImage = shopImage;
            ProductsProvidersEdit = productsProvidersEdit;
        }

        public ShopId ShopId => useCase_addProduct.ShopId;

        public IEnumerable<ProductShopEditInfo> ProductEditInfos { get; private set; }
        public ShopImage ShopImage { get; }
        public Func<ShopImage, IEnumerable<ProductShopEditInfo>> ProductsProvidersEdit { get; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            ProductEditInfos = ProductsProvidersEdit(ShopImage);
            useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopImage);
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_CheckStoreProducts();
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
            EditProducts();
            return CalculateExpected();
        }

        private void EditProducts()
        {
            foreach (ProductShopEditInfo productEditInfo in ProductEditInfos)
            {
                Assert.IsTrue(Bridge.EditProductInShop(ShopId, productEditInfo.ProductOriginal.ProductId, productEditInfo.ProductInfoEdit));
            }
        }

        private IEnumerable<ProductIdentifiable> CalculateExpected()
        {
            ISet<ProductIdentifiable> expected = new HashSet<ProductIdentifiable>(ShopImage.ShopProducts);
            foreach (ProductShopEditInfo productEditInfo in ProductEditInfos)
            {
                _ = expected.Remove(productEditInfo.ProductOriginal);
                _ = expected.Add(new ProductIdentifiable(productEditInfo.ProductInfoEdit, productEditInfo.ProductOriginal.ProductId));
            }
            return expected;
        }

        public void Failure_NotLoggedIn()
        {
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, productId, productInfo));
        }

        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, productId, productInfo));
        }

        public void Failure_ShopDoesntExist()
        {
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(Guid.NewGuid(), productId, productInfo));
        }

        public void Failure_InvalidShopId()
        {
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(default, productId, productInfo));
        }

        [TestCase]
        public void Failure_InvalidProductId()
        {
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, default, productInfo));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            ProductId productId = ShopImage.ShopProducts[0].ProductId;
            ProductInfo productInfo = ProductEditInfos.First().ProductInfoEdit;
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, new ProductId(Guid.NewGuid()), productInfo));
        }

        [TestCase]
        public void Failure_ProductNotInShop()
        {
            var product2 = new ProductIdentifiable(new ProductInfo
            (
                name: "air conditioner remote control",
                quantity: 800,
                price: 6,
                category: "air conditioning",
                weight: 0.6
            ));
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            useCase_addProduct = new UseCase_AddProductToShop
            (
                SystemContext,
                new ShopImage(UserInfo, Shop2, new ProductIdentifiable[] { product2 })
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_CheckStoreProducts();

            ProductInfo productInfo = product2.ProductInfo;
            productInfo.Price = 5;
            Assert.IsFalse(Bridge.EditProductInShop(ShopId, product2.ProductId, productInfo));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsFalse(Bridge.EditProductInShop
            (
                ShopId,
                ShopImage.ShopProducts[0].ProductId,
                new ProductInfo
                (
                    name: "ineditcucumber",
                    quantity: 23,
                    price: -7,
                    category: "cat1",
                    weight: 2
                )
            ));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsFalse(Bridge.EditProductInShop
            (
                ShopId,
                ShopImage.ShopProducts[0].ProductId,
                new ProductInfo
                (
                    name: "",
                    quantity: 23,
                    price: 14,
                    category: "cat1",
                    weight: 2
                )
            ));
        }
    }
}
