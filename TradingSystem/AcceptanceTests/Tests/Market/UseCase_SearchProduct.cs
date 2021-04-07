using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 4: Search product
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/52
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_SearchProduct : MarketMemberTestBase
    {
        public static readonly Func<ShopImage[]> DefaultMarketImageFactorry = () =>
            new ShopImage[]
            {
                new ShopImage(
                    new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                    new ShopInfo(SHOP_NAME),
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo("traveling bag", 20, 7)),
                    }
                ),
                new ShopImage(
                    new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                    new ShopInfo(SHOP_NAME),
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo("school bag", 5, 12)),
                        new ProductIdentifiable(new ProductInfo("android charger", 8, 20)),
                    }
                ),
            };

        private static readonly object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
                DefaultMarketImageFactorry
            },
        };

        private UseCase_Login useCase_login_buyer;

        public Func<ShopImage[]> MarketImageFactory { get; }
        public ShopImage[] MarketImage { get; private set; }
        private ShopTestState[] MarketState { get; set; }

        public UseCase_SearchProduct(
            SystemContext systemContext,
            UserInfo buyerUser,
            Func<ShopImage[]> marketImageFactory
        ) : base(systemContext, buyerUser)
        {
            MarketImageFactory = marketImageFactory;
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            PrepareMarket();
            useCase_login_buyer = new UseCase_Login(SystemContext, UserInfo);
            useCase_login_buyer.Setup();
            useCase_login_buyer.Success_Normal();
        }

        private void PrepareMarket()
        {
            MarketImage = MarketImageFactory();
            MarketState = MarketImage
                .Select(x => new ShopTestState(SystemContext, x))
                .ToArray();
            ShopTestState? prevShop = null;
            foreach (ShopTestState shop in MarketState)
            {
                prevShop?.Logout();
                shop.Login();
                shop.AddAllProducts();
                prevShop = shop;
            }
            prevShop?.Logout();
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_login_buyer?.TearDown();
            for (int i = MarketState.Length - 1; i >= 0; i--)
            {
                ShopTestState shop = MarketState[i];
                shop.Teardown();
            }
        }

        private void TestSuccess(string testName, ProductSearchCreteria searchCreteria, IEnumerable<ProductIdentifiable> expectedResults)
        {
            ProductSearchResults? results = Bridge.SearchProducts(searchCreteria);
            Assert.IsNotNull(results);
            Assert.IsNull(results!.TypoFixes, $"{testName}: expected no typo fixes");
            Assert.IsTrue(results!.IsValid(), $"{testName}: expect valid results (valid IDs)");
            new Assert_SetEquals<ProductIdentifiable>(
                testName,
                expectedResults,
                ProductIdentifiable.DeepEquals
            ).AssertEquals(results);
        }

        [TestCase]
        public void Success_MultipleShops()
        {
            TestSuccess(
                "Products search - success - multiple shops",
                new ProductSearchCreteria("bag")
                {
                    PriceRange_Low = 0.5m
                },
                new ProductIdentifiable[]
                {
                    MarketImage[0].ShopProducts[0],
                    MarketImage[1].ShopProducts[0],
                }
            );
        }

        [TestCase]
        public void Success_OneProduct()
        {
            TestSuccess(
                "Products search - success - one shop",
                new ProductSearchCreteria("charger")
                {
                    PriceRange_High = 21
                },
                new ProductIdentifiable[]
                {
                    MarketImage[1].ShopProducts[1]
                }
            );
        }

        [TestCase]
        public void Failure_MatchingKeywordsButNotFilter()
        {
            ProductSearchResults? results = Bridge.SearchProducts(new ProductSearchCreteria("charger")
            {
                PriceRange_High = 1
            });
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching filter - expected no results");
        }

        [TestCase]
        public void Failure_NotMatchingKeywords()
        {
            ProductSearchResults? results = Bridge.SearchProducts(new ProductSearchCreteria("suabjkbeio"));
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching keywords - expected no results");
        }

        [TestCase]
        public void Failure_Typo()
        {
            ProductSearchResults? results = Bridge.SearchProducts(new ProductSearchCreteria("abg"));
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching keywords - expected no results");
            Assert.AreEqual(results!.TypoFixes, "bag", "Product search - typo - expected fix");
        }

        private class ShopTestState
        {
            private UseCase_AddProduct? useCase_addProduct;

            public ShopTestState(SystemContext systemContext, ShopImage shopImage)
            {
                SystemContext = systemContext;
                ShopImage = shopImage;
            }

            public SystemContext SystemContext { get; }
            public ShopImage ShopImage { get; }

            public void AddAllProducts()
            {
                foreach (ProductIdentifiable product in ShopImage.ShopProducts)
                {
                    product.ProductId = useCase_addProduct!.Success_Normal(product.ProductInfo);
                }
            }

            public void Login()
            {
                useCase_addProduct = new UseCase_AddProduct(ShopImage.ShopInfo.Name, SystemContext, ShopImage.OwnerUser);
                useCase_addProduct.Setup();
            }

            public void Logout()
            {
                new UseCase_LogOut_TestLogic(SystemContext)
                    .Success_Normal();
            }

            public void Teardown()
            {
                if (!ShopImage.OwnerUser.Equals(SystemContext.LoggedInUser))
                {
                    _ = SystemContext.UserBridge.Login(ShopImage.OwnerUser);
                }
                useCase_addProduct?.Teardown();
            }
        }
    }
}
