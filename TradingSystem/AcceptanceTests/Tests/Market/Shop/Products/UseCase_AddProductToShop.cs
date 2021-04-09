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
    [TestFixture(SHOP_NAME, USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD)]
    public class UseCase_AddProductToShop : ShopManagementTestBase
    {
        private static readonly object[] TestProductInfo =
        {
            new object[]
            {
                new ProductInfo("cucumber", 4, 4),
            },
        };

        private UseCase_OpenShop useCase_openShop;
        private Queue<ProductId> products;

        public UseCase_AddProductToShop(string shopName, string shopUsername, string shopUserPassword) :
            this(shopName, SystemContext.Instance, new UserInfo(shopUsername, shopUserPassword))
        { }
        public UseCase_AddProductToShop(string shopName, SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        {
            ShopName = shopName;
            products = new Queue<ProductId>(3);
        }

        public string ShopName { get; }
        public ShopId ShopId { get; private set; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_openShop = new UseCase_OpenShop(SystemContext, UserInfo);
            useCase_openShop.Setup();
            ShopId = useCase_openShop.Success_Normal(new ShopInfo(ShopName));
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
            ProductId? product = Bridge.AddProductToShop(ShopId, productInfo);
            Assert.IsNotNull(product);
            Assert.Greater(product!.Value, 0);
            products.Enqueue(product.Value);
            return product.Value;
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("cucumber", 4, 3)));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("cucumber", -3, 3)));
        }

        [TestCase]
        public void Failure_InvalidQuantity()
        {
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("cucumber", 4, -1)));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("cucumber", 4, -5)));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("", 4, 3)));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("         ", 4, 3)));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("     \t    ", 4, 3)));
            Assert.IsNull(Bridge.AddProductToShop(ShopId, new ProductInfo("    \n \t", 4, 3)));
        }

        [TestCaseSource(nameof(TestProductInfo))]
        public void Failure_InvalidShopId(ProductInfo productInfo)
        {
            Assert.IsNull(Bridge.AddProductToShop(-1, productInfo));
            Assert.IsNull(Bridge.AddProductToShop(-6, productInfo));
        }
    }
}
