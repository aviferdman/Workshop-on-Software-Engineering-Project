using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 5: Add product to shopping basket
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_AddProductToCart : MarketTestBase
    {
        private static object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
                new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                SHOP_NAME,
                new ProductInfo("speakers", 30, 90),
            },
        };

        public UserInfo Shop_Owner_User { get; }
        public string ShopName { get; }
        public ProductInfo ProductInfo { get; }

        public UseCase_AddProductToCart(
            SystemContext systemContext,
            UserInfo buyer_user,
            UserInfo shop_owner_user,
            string shopName,
            ProductInfo productInfo
        ) : base(systemContext, buyer_user)
        {
            Shop_Owner_User = shop_owner_user;
            ShopName = shopName;
            ProductInfo = productInfo;
        }

        private UseCase_AddProduct useCase_addProduct;
        private UseCase_Login useCase_login_buyer;
        private Product product;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            useCase_addProduct = new UseCase_AddProduct(ShopName, SystemContext, Shop_Owner_User);
            useCase_addProduct.Setup();
            product = useCase_addProduct.Success_Normal(ProductInfo);

            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            useCase_login_buyer = new UseCase_Login(SystemContext, UserInfo);
            useCase_login_buyer.Setup();
            useCase_login_buyer.Success_Normal();
        }

        [TearDown]
        public void Teardown()
        {
            useCase_addProduct?.Teardown();
            useCase_login_buyer?.TearDown();
        }

        [TestCase]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.AddProductToUserCart(product));
        }
    }
}
