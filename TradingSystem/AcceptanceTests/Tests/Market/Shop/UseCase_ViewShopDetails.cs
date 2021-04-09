
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop
{
    /// <summary>
    /// Acceptance test for
    /// Use case 20: View shop’s details
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_ViewShopDetails : MarketTestBase
    {
        static object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_SHOP_OWNER_NAME, USER_BUYER_PASSWORD),
                new ShopInfo(SHOP_NAME),
            },
        };

        public UseCase_ViewShopDetails(SystemContext systemContext, UserInfo shopOwnerUser, ShopInfo shopInfo) :
            base(systemContext)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopInfo = shopInfo;
        }

        public UserInfo ShopOwnerUser { get; }
        public ShopInfo ShopInfo { get; }

        private UseCase_OpenShop useCase_openShop;
        private ShopId shopId;

        [SetUp]
        public void Setup()
        {
            useCase_openShop = new UseCase_OpenShop(SystemContext, ShopOwnerUser);
            useCase_openShop.Setup();
            shopId = useCase_openShop.Success_Normal(ShopInfo);
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
        }

        [TearDown]
        public void Teardown()
        {
            useCase_openShop.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            ShopInfo? returnedShopInfo = Bridge.GetShopDetails(shopId);
            Assert.IsNotNull(returnedShopInfo);
            Assert.AreEqual(ShopInfo.Name, returnedShopInfo!.Name);
        }

        [TestCase]
        public void Failure_ShopDoesNotExist()
        {
            ShopInfo? returnedShopInfo = Bridge.GetShopDetails(int.MaxValue - 1);
            Assert.IsNull(returnedShopInfo);
        }
    }
}
