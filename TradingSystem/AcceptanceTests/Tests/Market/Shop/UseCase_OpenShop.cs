using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop
{
    /// <summary>
    /// Acceptance test for
    /// Use case 22: Open shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_OpenShop : ShopManagementTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User_ShopOwner1,
            },
        };

        public UseCase_Login useCase_login;

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
        public override void Teardown()
        {
            useCase_login?.Teardown();
        }

        private static readonly object[] Args_Sucess_Normal_Source =
        {
            new object[]
            {
                Shop1,
            },
        };

        [Test]
        public void Success_Normal_Test(ShopInfo shopInfo)
        {
            _ = Success_Normal(shopInfo);
        }
        public ShopId Success_Normal(ShopInfo shopInfo)
        {
            ShopId? shop = Bridge.AssureOpenShop(shopInfo);
            Assert.IsNotNull(shop);
            Assert.Greater(shop!.Value, 0);
            return shop.Value;
        }

        [TestCase]
        public void Failure_NotLoggedIn()
        {
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            Assert.IsNull(Bridge.OpenShop(new ShopInfo(
                name: "non existing shop",
                branch: 3,
                new Address
                {
                    State = "Victoria Island",
                    City = "Henesys",
                    Street = "Free Market",
                    ApartmentNum = "52",
                }
            )));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsNull(Bridge.OpenShop(new ShopInfo(
                name: "",
                branch: 3,
                new Address
                {
                    State = "Victoria Island",
                    City = "Ellinia",
                    Street = "Giant Tree",
                    ApartmentNum = "52",
                }
            )));
            Assert.IsNull(Bridge.OpenShop(new ShopInfo(
                name: "    ",
                branch: 3,
                new Address
                {
                    State = "Victoria Island",
                    City = "Perion",
                    Street = "Dusty Wind Hill",
                    ApartmentNum = "52",
                }
            )));
            Assert.IsNull(Bridge.OpenShop(new ShopInfo(
                name: "    \n  \t",
                branch: 3,
                new Address
                {
                    State = "Victoria Island",
                    City = "Perion",
                    Street = "Dusty Wind Hill",
                    ApartmentNum = "52",
                }
            )));
        }
    }
}
