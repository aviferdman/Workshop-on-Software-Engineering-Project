using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

namespace AcceptanceTests.Tests.Market.Shop
{
    public class ShopManagementTestBase : MarketMemberTestBase
    {
        public ShopManagementTestBase(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        protected void LoginToBuyer()
        {
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            _ = Login(User_Buyer);
        }
    }
}
