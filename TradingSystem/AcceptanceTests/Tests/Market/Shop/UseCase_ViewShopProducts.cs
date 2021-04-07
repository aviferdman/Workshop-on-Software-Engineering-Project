using System;
using System.Collections.Generic;
using System.Linq;
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
                new ProductIdentifiable[]
                {
                    new ProductIdentifiable(new ProductInfo("suitcase", 200, 30)),
                    new ProductIdentifiable(new ProductInfo("hdmi cable", 7, 321)),
                }
            },
        };

        public UseCase_ViewShopProducts(SystemContext systemContext, UserInfo shopOwnerUser, ShopInfo shopInfo, ProductIdentifiable[] products) :
            base(systemContext)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopInfo = shopInfo;
            Products = products;
        }

        public UserInfo ShopOwnerUser { get; }
        public ShopInfo ShopInfo { get; }
        public ProductIdentifiable[] Products { get; }

        private UseCase_AddProduct useCase_addProduct;

        [SetUp]
        public void Setup()
        {
            useCase_addProduct = new UseCase_AddProduct(ShopInfo.Name, SystemContext, ShopOwnerUser);
            useCase_addProduct.Setup();

            foreach (ProductIdentifiable product in Products)
            {
                product.ProductId = useCase_addProduct.Success_Normal(product.ProductInfo);
            }

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
            IEnumerable<ProductIdentifiable>? resultProducts = Bridge.GetShopProducts(useCase_addProduct.ShopId);
            Assert.IsNotNull(resultProducts);
            Assert.IsTrue(resultProducts.All(x => x.ProductId.IsValid()), "View shopping cart - success - contains invalid product IDs");
            new Assert_SetEquals<ProductIdentifiable>(
                "View shopping cart - success",
                Products,
                ProductIdentifiable.DeepEquals
            ).AssertEquals(resultProducts);
        }

        [TestCase]
        public void Failure_ShopDoesNotExist()
        {
            ShopInfo? returnedShopInfo = Bridge.GetShopDetails(int.MaxValue - 1);
            Assert.IsNull(returnedShopInfo);
        }
    }
}
