using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    public class MarketTestBase
    {
        public const string USER_SHOP_OWNER_NAME = SharedTestsConstants.USER_SHOP_OWNER_NAME;
        public const string USER_SHOP_OWNER_PASSWORD = SharedTestsConstants.USER_SHOP_OWNER_PASSWORD;

        public const string USER_BUYER_NAME = SharedTestsConstants.USER_BUYER_NAME;
        public const string USER_BUYER_PASSWORD = SharedTestsConstants.USER_BUYER_PASSWORD;

        public const string SHOP_NAME = SharedTestsConstants.SHOP_NAME;

        public SystemContext SystemContext { get; }

        public UserInfo UserInfo { get; }

        public MarketTestBase(SystemContext systemContext, UserInfo userInfo)
        {
            SystemContext = systemContext;
            UserInfo = userInfo;
        }

        protected IMarketBridge Bridge => SystemContext.MarketBridge;

        [SetUp]
        public virtual void Setup() { }

        protected UseCase_Login Login()
        {
            return Login(UserInfo);
        }

        protected UseCase_Login Login(UserInfo userInfo)
        {
            var useCase_login = new UseCase_Login(SystemContext, userInfo);
            useCase_login.Setup();
            useCase_login.Success_Normal();
            return useCase_login;
        }
    }
}
