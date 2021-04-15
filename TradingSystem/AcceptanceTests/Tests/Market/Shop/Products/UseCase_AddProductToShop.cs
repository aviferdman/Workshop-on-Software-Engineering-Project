using System;
using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
                User_ShopOwner1,
                Shop1,
            },
        };

        private static readonly object[] TestProductInfo =
        {
            new object[]
            {
                new ProductInfo(
                    name: "tomato",
                    quantity: 4,
                    price: 4,
                    category: "fruits",
                    weight: 0.7
                ),
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

        public UseCase_AddProductToShop(SystemContext systemContext, UserInfo userInfo, ShopInfo shopInfo) :
            base(systemContext, userInfo)
        {
            ShopInfo = shopInfo;
            products = new Queue<ProductId>(3);
        }

        public ShopInfo ShopInfo { get; }
        public ShopId ShopId { get; private set; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_openShop = new UseCase_OpenShop(SystemContext, UserInfo);
            useCase_openShop.Setup();
            ShopId = useCase_openShop.Success_Normal(ShopInfo);
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

        [TestCaseSource(nameof(TestProductInfo))]
        public void Success_Normal_Test(ProductInfo productInfo)
        {
            _ = Success_Normal(productInfo);
        }
        public ProductId Success_Normal(ProductInfo productInfo)
        {
            ProductId? productId = Bridge.AddProductToShop(ShopId, productInfo);
            Assert.IsNotNull(productId);
            // TODO: check the store products here
            products.Enqueue(productId!.Value);
            return productId.Value;
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_InsufficientPermissions(ProductInfo productInfo)
        {
            LoginToBuyer();
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
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
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, productInfo));
        }
    }
}
