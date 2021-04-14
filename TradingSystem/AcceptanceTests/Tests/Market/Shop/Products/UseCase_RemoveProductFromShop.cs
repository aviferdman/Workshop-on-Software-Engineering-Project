
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

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
                User_ShopOwner1,
                Shop1,
            },
        };

        private UseCase_AddProductToShop useCase_addProduct;
        private ProductId product;

        public UseCase_RemoveProductFromShop(SystemContext systemContext, UserInfo userInfo, ShopInfo shopInfo) :
            base(systemContext, userInfo)
        {
            ShopInfo = shopInfo;
        }

        public ShopInfo ShopInfo { get; }
        public ShopId ShopId => useCase_addProduct.ShopId;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_addProduct = new UseCase_AddProductToShop(SystemContext.Instance, UserInfo, ShopInfo);
            useCase_addProduct.Setup();
            product = useCase_addProduct.Success_Normal(new ProductInfo("cucumber", 4, 7));
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_addProduct.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.RemoveProductFromShop(ShopId, product));
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsFalse(Bridge.RemoveProductFromShop(ShopId, product));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            Assert.IsFalse(Bridge.RemoveProductFromShop(ShopId, new ProductId(int.MaxValue - 1)));
        }
    }
}
