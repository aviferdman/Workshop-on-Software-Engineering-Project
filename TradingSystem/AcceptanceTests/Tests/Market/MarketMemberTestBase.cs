using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market
{
    public class MarketMemberTestBase : MarketTestBase
    {
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
