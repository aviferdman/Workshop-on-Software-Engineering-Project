using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market
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
                    User_ShopOwner1,
                    Shop1,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo(
                            name: "traveling bag",
                            quantity: 7,
                            price: 20,
                            category: "bags",
                            weight: 1
                        )),
                    }
                ),
                new ShopImage(
                    User_ShopOwner1,
                    Shop2,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo(
                            name: "school bag",
                            quantity: 12,
                            price: 5,
                            category: "bags",
                            weight: 0.8
                        )),
                        new ProductIdentifiable(new ProductInfo(
                            name: "android charger",
                            quantity: 20,
                            price: 8,
                            category: "phone chargers",
                            weight: 0.09
                        )),
                    }
                ),
            };

        private static readonly object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                User_Buyer,
                DefaultMarketImageFactorry
            },
        };

        private UseCase_Login useCase_login_buyer;

        public Func<ShopImage[]> MarketImageFactory { get; }
        public ShopImage[] MarketImage { get; private set; }
        private ShopTestState[] MarketState { get; set; }

        public UseCase_SearchProduct
        (
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
            useCase_login_buyer?.Teardown();
            for (int i = MarketState.Length - 1; i >= 0; i--)
            {
                ShopTestState shop = MarketState[i];
                shop.Teardown();
            }
        }

        private void TestSuccess(ProductSearchCreteria searchCreteria, IEnumerable<ProductIdentifiable> expectedResults)
        {
            ProductSearchResults? results = MarketBridge.SearchProducts(searchCreteria);
            Assert.IsNotNull(results);
            Assert.IsNull(results!.TypoFixes, $"expected no typo fixes");
            Assert.IsTrue(results!.IsValid(), $"expect valid results (valid IDs)");
            new Assert_SetEquals<ProductIdentifiable>
            (
                expectedResults,
                ProductIdentifiable.DeepEquals
            ).AssertEquals(results);
        }

        [TestCase]
        public void Success_MultipleShops()
        {
            TestSuccess
            (
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
            TestSuccess
            (
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
            ProductSearchResults? results = MarketBridge.SearchProducts(new ProductSearchCreteria("charger")
            {
                PriceRange_High = 1
            });
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching filter - expected no results");
        }

        [TestCase]
        public void Failure_NotMatchingKeywords()
        {
            ProductSearchResults? results = MarketBridge.SearchProducts(new ProductSearchCreteria("suabjkbeio"));
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching keywords - expected no results");
        }

        // Not a test case for now
        public void Failure_Typo()
        {
            ProductSearchResults? results = MarketBridge.SearchProducts(new ProductSearchCreteria("abg"));
            Assert.NotNull(results, "Products search - null results");
            Assert.IsEmpty(results, "Products search - not matching keywords - expected no results");
            Assert.AreEqual(results!.TypoFixes, "bag", "Product search - typo - expected fix");
        }

        private class ShopTestState
        {
            private UseCase_AddProductToShop? useCase_addProduct;

            public ShopTestState(SystemContext systemContext, ShopImage shopImage)
            {
                SystemContext = systemContext;
                ShopImage = shopImage;
            }

            public SystemContext SystemContext { get; }
            public ShopImage ShopImage { get; }

            public void AddAllProducts()
            {
                useCase_addProduct!.Success_Normal_CheckStoreProducts();
            }

            public void Login()
            {
                useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopImage);
                useCase_addProduct.Setup();
            }

            public void Logout()
            {
                new UseCase_LogOut_TestLogic(SystemContext)
                    .Success_Normal();
            }

            public void Teardown()
            {
                useCase_addProduct?.Teardown();
            }
        }
    }
}
