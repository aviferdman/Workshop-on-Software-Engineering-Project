using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests.Shop
{
    /// <summary>
    /// Acceptance test for
    /// Use case 21: View shop’s products
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/79
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_ViewShopProducts : MarketTestBase
    {
        static object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_SHOP_OWNER_NAME, USER_BUYER_PASSWORD),
                new ShopInfo(SHOP_NAME),
            },
        };

        public UseCase_ViewShopProducts(SystemContext systemContext, UserInfo shopOwnerUser, ShopInfo shopInfo) :
            base(systemContext)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopInfo = shopInfo;
        }

        public UserInfo ShopOwnerUser { get; }
        public ShopInfo ShopInfo { get; }

        private UseCase_AddProduct useCase_addProduct;
        private IList<ProductId> products;

        [SetUp]
        public void Setup()
        {
            ProductId productId;
            products = new List<ProductId>(2);
            useCase_addProduct = new UseCase_AddProduct(ShopInfo.Name, SystemContext, ShopOwnerUser);
            useCase_addProduct.Setup();

            productId = useCase_addProduct.Success_Normal(new ProductInfo("suitcase", 200, 30));
            products.Add(productId);

            productId = useCase_addProduct.Success_Normal(new ProductInfo("hdmi cable", 7, 321));
            products.Add(productId);

            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
        }

        [TearDown]
        public void Teardown()
        {
            useCase_addProduct?.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            new Assert_SetEquals<ProductId>("View shop products - success", products)
                .AssertEquals(Bridge.GetShopProducts(useCase_addProduct.ShopId));
        }

        [TestCase]
        public void Failure_ShopDoesNotExist()
        {
            ShopInfo? returnedShopInfo = Bridge.GetShopDetails(int.MaxValue - 1);
            Assert.IsNull(returnedShopInfo);
        }
    }
}
