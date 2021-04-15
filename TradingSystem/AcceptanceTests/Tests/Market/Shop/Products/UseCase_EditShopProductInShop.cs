using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
                (Func<ShopImage, IEnumerable<ProductEditInfo>>)(
                    shopImage =>
                    new ProductEditInfo[]
                    {
                        new ProductEditInfo(
                            shopImage.ShopProducts[0],
                            new ProductInfo(
                                name: "WiiU",
                                quantity: 90,
                                price: 1100,
                                category: "gaming consoles",
                                weight: 5
                            )
                        ),
                        new ProductEditInfo(
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
            Func<ShopImage, IEnumerable<ProductEditInfo>> productsProvidersEdit
        ) :
            base(systemContext, shopImage.OwnerUser)
        {
            ShopImage = shopImage;
            ProductsProvidersEdit = productsProvidersEdit;
        }

        public ShopId ShopId => useCase_addProduct.ShopId;

        public IEnumerable<ProductEditInfo> ProductEditInfos { get; private set; }
        public ShopImage ShopImage { get; }
        public Func<ShopImage, IEnumerable<ProductEditInfo>> ProductsProvidersEdit { get; }

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
            foreach (ProductEditInfo productEditInfo in ProductEditInfos)
            {
                Assert.IsTrue(Bridge.EditProductInShop(ShopId, productEditInfo.ProductOriginal.ProductId, productEditInfo.ProductInfoEdit));
            }
        }

        private IEnumerable<ProductIdentifiable> CalculateExpected()
        {
            ISet<ProductIdentifiable> expected = new HashSet<ProductIdentifiable>(ShopImage.ShopProducts);
            foreach (ProductEditInfo productEditInfo in ProductEditInfos)
            {
                expected.Remove(productEditInfo.ProductOriginal);
                expected.Add(new ProductIdentifiable(productEditInfo.ProductInfoEdit, productEditInfo.ProductOriginal.ProductId));
            }
            return expected;
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsFalse(Bridge.EditProductInShop(
                ShopId,
                ShopImage.ShopProducts[0].ProductId,
                ProductEditInfos.First().ProductInfoEdit
            ));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            Assert.IsFalse(Bridge.EditProductInShop(
                ShopId,
                default,
                ProductEditInfos.First().ProductInfoEdit
            ));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsFalse(Bridge.EditProductInShop(
                ShopId,
                ShopImage.ShopProducts[0].ProductId,
                new ProductInfo(
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
            Assert.IsFalse(Bridge.EditProductInShop(
                ShopId,
                ShopImage.ShopProducts[0].ProductId,
                new ProductInfo(
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
