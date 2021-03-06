using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;
using TradingSystem.Business.Market;

namespace AcceptanceTests.Tests.User
{
    /// <summary>
    /// Acceptance test for
    /// Use case 1: Sign Up
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/11
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_SignUp : MemberUseTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User,
            },
        };

        public UseCase_SignUp(SystemContext systemContext, UserInfo signupInfo) :
            base(systemContext, signupInfo)
        { }

        public override void Setup()
        {
            base.Setup();
            SystemContext.MarketBridge.SetDbDebugMode(true);
            Assert.IsTrue(Bridge.Connect());
        }

        public override void Teardown()
        {
            _ = Bridge.Disconnect();
            base.Teardown();
        }

        [Test]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.AssureSignUp(UserInfo));
        }

        [Test]
        public void Failure_UsernameTaken()
        {
            Success_Normal();
            Assert.IsFalse(Bridge.SignUp(UserInfo.WithDifferentPassword("abcd1234")));
        }

        
    }
}