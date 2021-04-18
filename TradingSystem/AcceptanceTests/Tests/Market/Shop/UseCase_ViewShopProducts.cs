using System;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.Shop
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
                new ShopImage(
                    User_ShopOwner1,
                    Shop1,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo(
                            name: "suitcase",
                            quantity: 30,
                            price: 200,
                            category: "suitcases",
                            weight: 4
                        )),
                        new ProductIdentifiable(new ProductInfo(
                            name: "hdmi cable",
                            quantity: 321,
                            price: 7,
                            category: "computer cables",
                            weight: 0.12
                        )),
                    }
                )
            },
        };

        public UseCase_ViewShopProducts(SystemContext systemContext, ShopImage shopImage) :
            base(systemContext)
        {
            ShopImage = shopImage;
        }

        private UseCase_AddProductToShop useCase_addProduct;
        private UseCase_ViewShopProducts_TestLogic testLogic;

        public ShopImage ShopImage { get; }

        [SetUp]
        public void Setup()
        {
            useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopImage);
            useCase_addProduct.Setup();
            useCase_addProduct.Success_Normal_NoCheckStoreProducts();

            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            testLogic = new UseCase_ViewShopProducts_TestLogic(SystemContext);
        }

        [TearDown]
        public void Teardown()
        {
            useCase_addProduct?.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            testLogic!.Success_Normal(useCase_addProduct.ShopId, ShopImage.ShopProducts);
        }

        [TestCase]
        public void Failure_ShopDoesNotExist()
        {
            ShopInfo? returnedShopInfo = Bridge.GetShopDetails(new ShopId(Guid.NewGuid(), "notexists"));
            Assert.IsNull(returnedShopInfo);
        }
    }
}
