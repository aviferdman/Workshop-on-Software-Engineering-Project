using System;
using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    /// <summary>
    /// Acceptance test for
    /// Use case 23: add product to shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/18
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_AddProductToShop : ShopManagementTestBase
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
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "tomato",
                            quantity: 4,
                            price: 4,
                            category: "fruits",
                            weight: 0.7
                        )),
                    }
                )
            },
        };

        private static readonly object[] TestProductInfo =
        {
            new object[]
            {
                ((ShopImage)((object[])FixtureArgs[0])[1]).ShopProducts[0].ProductInfo
            },
        };

        private static readonly object[] InvalidQuantitySource =
        {
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                0,
            },
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                -1,
            },
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                -5,
            },
        };

        private static readonly object[] InvalidNameSource =
        {
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                "",
            },
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                "         ",
            },
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                "     \t    ",
            },
            new object[]
            {
                ((object[])TestProductInfo[0])[0],
                "    \n \t",
            },
        };

        private UseCase_OpenShop useCase_openShop;
        private Queue<ProductId> products;

        public UseCase_AddProductToShop(SystemContext systemContext, ShopImage shopImage) :
            base(systemContext, shopImage.OwnerUser)
        {
            ShopImage = shopImage;
            products = new Queue<ProductId>(shopImage.ShopProducts.Length);
        }

        public ShopImage ShopImage { get; }
        public ShopId ShopId { get; private set; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_openShop = new UseCase_OpenShop(SystemContext, UserInfo);
            useCase_openShop.Setup();
            ShopId = useCase_openShop.Success_Normal(ShopImage.ShopInfo);
        }

        [TearDown]
        public override void Teardown()
        {
            while (products.Count > 0)
            {
                _ = Bridge.RemoveProductFromShop(ShopId, products.Dequeue());
            }
            useCase_openShop.Teardown();
        }

        [TestCase]
        public void Success_Normal_CheckStoreProducts()
        {
            Success_Normal_CheckStoreProducts(ShopImage.ShopProducts, ShopImage.ShopProducts);
        }
        public void Success_Normal_NoCheckStoreProducts()
        {
            Success_Normal_NoCheckStoreProducts(ShopImage.ShopProducts);
        }

        public void Success_Normal_NoCheckStoreProducts(IEnumerable<ProductIdentifiable> products)
        {
            foreach (ProductIdentifiable product in products)
            {
                product.ProductId = Success_Normal_NoCheckStoreProducts(product.ProductInfo);
            }
        }
        public void Success_Normal_CheckStoreProducts
        (
            IEnumerable<ProductIdentifiable> products,
            IEnumerable<ProductIdentifiable> expectedStoreProducts
        )
        {
            Success_Normal_NoCheckStoreProducts(products);
            new UseCase_ViewShopProducts_TestLogic(SystemContext)
                .Success_Normal(ShopId, expectedStoreProducts);
        }

        public ProductId Success_Normal_NoCheckStoreProducts(ProductInfo productInfo)
        {
            ProductId? productId = Bridge.AddProductToShop(ShopId, productInfo);
            Assert.IsNotNull(productId);
            products.Enqueue(productId!.Value);
            return productId.Value;
        }
        public ProductId Success_Normal_CheckStoreProducts
        (
            ProductInfo productInfo,
            IEnumerable<ProductIdentifiable> expectedStoreProducts
        )
        {
            ProductId productId = Success_Normal_NoCheckStoreProducts(productInfo);
            new UseCase_ViewShopProducts_TestLogic(SystemContext)
                .Success_Normal(ShopId, expectedStoreProducts);
            return productId;
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_NotLoggedIn(ProductInfo productInfo)
        {
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_InsufficientPermissions(ProductInfo productInfo)
        {
            LoginToBuyer();
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_ShopDoesntExist(ProductInfo productInfo)
        {
            Assert.IsNull(Bridge.AddProductToShop(Guid.NewGuid(), productInfo));
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_InvalidShopId(ProductInfo productInfo)
        {
            Assert.IsNull(Bridge.AddProductToShop(default, productInfo));
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_InvalidPrice(ProductInfo productInfo)
        {
            productInfo.Price = -3;
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }

        [TestCaseSource(nameof(InvalidQuantitySource))]
        public void Failure_InvalidQuantity(ProductInfo productInfo, int quantity)
        {
            productInfo.Quantity = quantity;
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }

        [TestCaseSource(nameof(InvalidNameSource))]
        public void Failure_InvalidName(ProductInfo productInfo, string name)
        {
            productInfo.Name = name;
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }
    }
}
