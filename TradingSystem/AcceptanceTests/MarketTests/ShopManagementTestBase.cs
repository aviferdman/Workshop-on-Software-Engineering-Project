using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.MarketTests
{
    public class ShopManagementTestBase : MarketMemberTestBase
    {
        public ShopManagementTestBase(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        protected void LoginToBuyer()
        {
            _ = SystemContext.UserBridge.LogOut();
            _ = Login(new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD));
        }
    }
}
