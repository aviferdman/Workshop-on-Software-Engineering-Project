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
        public UseCase_OpenShop(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
        public UseCase_OpenShop(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        [Test]
        [TestCase("my shop 1")]
        public void Success_Normal_Test(string shopName)
        {
            _ = Success_Normal(shopName);
        }
        public int Success_Normal(string shopName)
        {
            int shopId = Bridge.OpenShop(shopName);
            Assert.Greater(shopId, 0);
            return shopId;
        }
    }
}
