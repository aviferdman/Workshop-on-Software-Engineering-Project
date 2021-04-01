using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 22: Open shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
    [TestFixture(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD)]
    public class UseCase_OpenShop : ShopManagementTestBase
    {
        public UseCase_Login useCase_login;

        public UseCase_OpenShop(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
        public UseCase_OpenShop(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_login = Login();
        }

        [TearDown]
        public void Teardown()
        {
            useCase_login?.TearDown();
        }

        [Test]
        [TestCase(SHOP_NAME)]
        public void Success_Normal_Test(string shopName)
        {
            _ = Success_Normal(new ShopInfo(shopName));
        }
        public ShopId Success_Normal(ShopInfo shopInfo)
        {
            ShopId? shop = Bridge.AssureOpenShop(shopInfo);
            Assert.IsNotNull(shop);
            Assert.Greater(shop!.Value, 0);
            return shop;
        }
    }
}
