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
        public static readonly ShopImage[] DefaultMarketImage = new ShopImage[]
        {
            new ShopImage(
                new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                new ShopInfo(SHOP_NAME),
                new ProductInfo[]
                {
                    new ProductInfo("traveling bag", 20, 7),
                }
            ),
            new ShopImage(
                new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                new ShopInfo(SHOP_NAME),
                new ProductInfo[]
                {
                    new ProductInfo("school bag", 5, 12),
                    new ProductInfo("android charger", 8, 20),
                }
            ),
        };

        private static readonly object[] FixtureArgs = new object[]
        {
            new object[]
            {
                SystemContext.Instance,
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
                DefaultMarketImage
            },
        };

        private UseCase_Login useCase_login_buyer;

        private ShopTestState[] MarketState { get; }
        public ShopImage[] MarketImage { get; }
        public IList<IList<ProductId>> Products { get; private set; }

        public UseCase_SearchProduct(
            SystemContext systemContext,
            UserInfo buyerUser,
            ShopImage[] marketImage
        ) : base(systemContext, buyerUser)
        {
            MarketImage = marketImage;
            MarketState = marketImage
                .Select(x => new ShopTestState(SystemContext, x))
                .ToArray();
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
            Products = new List<IList<ProductId>>();
            ShopTestState? prevShop = null;
            foreach (ShopTestState shop in MarketState)
            {
                prevShop?.Logout();
                shop.Login();
                Products.Add(shop.AddAllProducts());
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

        [TestCase]
        public void Success_MultipleShops()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("bag")
            {
                PriceRange_Low = 0.5m
            });
            new Assert_SetEquals<ProductId>("Products search - success multiple shops", Products[0][0], Products[1][0])
                .AssertEquals(products);
        }

        [TestCase]
        public void Success_OneProduct()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("charger")
            {
                PriceRange_High = 21
            });
            new Assert_SetEquals<ProductId>("Products search - success one shop", Products[1][1])
                .AssertEquals(products);
        }

        [TestCase]
        public void Failure_MatchingKeywordsButNotFilter()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("charger")
            {
                PriceRange_High = 1
            });
            Assert.NotNull(products, "Products search - null results");
            Assert.IsEmpty(products, "Products search - not matching filter - expected no results");
        }

        [TestCase]
        public void Failure_NotMatchingKeywords()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("suabjkbeio"));
            Assert.NotNull(products, "Products search - null results");
            Assert.IsEmpty(products, "Products search - not matching keywords - expected no results");
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

            public IList<ProductId> AddAllProducts()
            {
                var products = new List<ProductId>();
                foreach (ProductInfo productInfo in ShopImage.ShopProducts)
                {
                    ProductId productId = useCase_addProduct!.Success_Normal(productInfo);
                    products.Add(productId);
                }
                return products;
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
