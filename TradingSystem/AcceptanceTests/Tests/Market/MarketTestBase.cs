using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface.UserBridge;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market
{
    public class MarketTestBase
    {
        public static readonly BuyerUserInfo User_Buyer = SharedTestsData.User_Buyer;
        public static readonly BuyerUserInfo User_Buyer2 = SharedTestsData.User_Buyer2;
        public static readonly UserInfo User_ShopOwner1 = SharedTestsData.User_ShopOwner1;
        public static readonly UserInfo User_ShopOwner2 = SharedTestsData.User_ShopOwner2;

        public static readonly ShopInfo Shop1 = SharedTestsData.Shop1;
        public static readonly ShopInfo Shop2 = SharedTestsData.Shop2;

        public SystemContext SystemContext { get; }

        public MarketTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        [SetUp]
        public virtual void Setup() { }

        [TearDown]
        public virtual void Teardown() { }

        protected IMarketBridge MarketBridge => SystemContext.MarketBridge;
        protected IUserBridge UserBridge => SystemContext.UserBridge;

        protected UseCase_Login Login(UserInfo userInfo)
        {
            var useCase_login = new UseCase_Login(SystemContext, userInfo);
            useCase_login.Setup();
            useCase_login.Success_Normal();
            return useCase_login;
        }

        protected UseCase_Login LoginAssure(UserInfo userInfo)
        {
            var useCase_login = new UseCase_Login(SystemContext, userInfo);
            useCase_login.Setup();
            useCase_login.Success_Assure();
            return useCase_login;
        }
    }
}
