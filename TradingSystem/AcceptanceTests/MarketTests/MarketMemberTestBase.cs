using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    public class MarketMemberTestBase : MarketTestBase
    {
        public const string USER_SHOP_OWNER_NAME = SharedTestsConstants.USER_SHOP_OWNER_NAME;
        public const string USER_SHOP_OWNER_PASSWORD = SharedTestsConstants.USER_SHOP_OWNER_PASSWORD;

        public const string USER_SHOP_OWNER_2_NAME = SharedTestsConstants.USER_SHOP_OWNER_2_NAME;
        public const string USER_SHOP_OWNER_2_PASSWORD = SharedTestsConstants.USER_SHOP_OWNER_2_PASSWORD;

        public const string USER_BUYER_NAME = SharedTestsConstants.USER_BUYER_NAME;
        public const string USER_BUYER_PASSWORD = SharedTestsConstants.USER_BUYER_PASSWORD;

        public const string SHOP_NAME = SharedTestsConstants.SHOP_NAME;
        public const string SHOP_NAME_2 = SharedTestsConstants.SHOP_NAME_2;

        public UserInfo UserInfo { get; }

        public MarketMemberTestBase(SystemContext systemContext, UserInfo userInfo)
            : base(systemContext)
        {
            UserInfo = userInfo;
        }

        [SetUp]
        public virtual void Setup() { }

        [TearDown]
        public virtual void Teardown() { }

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
