using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 4: Search product
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/52
    /// </summary>
    [TestFixture(USER_BUYER_NAME, USER_BUYER_PASSWORD)]
    public class UseCase_SearchProduct : MarketMemberTestBase
    {
        private UseCase_AddProduct useCase_addProduct_shop1;
        public ProductId Product1_1 { get; set; }

        private UseCase_AddProduct useCase_addProduct_shop2;
        public ProductId Product2_2 { get; set; }
        public ProductId Product2_1 { get; set; }

        private UseCase_Login useCase_login_buyer;

        private bool logged_out_from_first;
        private bool logged_out_from_second;

        public UseCase_SearchProduct(string username, string password) :
            this(SystemContext.Instance, new UserInfo(username, password))
        { }
        public UseCase_SearchProduct(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            logged_out_from_first = false;
            Product1_1 = AddProduct(
                SHOP_NAME,
                USER_SHOP_OWNER_NAME,
                USER_SHOP_OWNER_PASSWORD,
                new ProductInfo("traveling bag", 20, 7),
                out useCase_addProduct_shop1
            );
            new UseCase_LogOut_TestLogic(SystemContext)
                .Success_Normal();
            logged_out_from_first = true;
            Product2_1 = AddProduct(
                SHOP_NAME_2,
                USER_SHOP_OWNER_2_NAME,
                USER_SHOP_OWNER_2_PASSWORD,
                new ProductInfo("school bag", 5, 12),
                out useCase_addProduct_shop2
            );
            Product2_2 = useCase_addProduct_shop2.Success_Normal(new ProductInfo("android charger", 8, 20));
            new UseCase_LogOut_TestLogic(SystemContext)
                .Success_Normal();
            useCase_login_buyer = new UseCase_Login(SystemContext, UserInfo);
            useCase_login_buyer.Setup();
            useCase_login_buyer.Success_Normal();
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_login_buyer?.TearDown();
            if (logged_out_from_second)
            {
                _ = Login(new UserInfo(USER_SHOP_OWNER_2_NAME, USER_SHOP_OWNER_2_PASSWORD));
            }
            useCase_addProduct_shop2?.Teardown();
            if (logged_out_from_first)
            {
                _ = Login(new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD));
            }
            useCase_addProduct_shop1?.Teardown();
        }

        private ProductId AddProduct(string shopName, string shopUsername, string shopUserPassword, ProductInfo productInfo, out UseCase_AddProduct useCase_addProduct)
        {
            useCase_addProduct = new UseCase_AddProduct(shopName, SystemContext, new UserInfo(shopUsername, shopUserPassword));
            useCase_addProduct.Setup();
            return useCase_addProduct.Success_Normal(productInfo);
        }

        [TestCase]
        public void Success_MultipleShops()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("bag")
            {
                PriceRange_Low = 0.5m
            });
            new Assert_SetEquals<ProductId>("Products search - success multiple shops", Product1_1, Product2_1)
                .AssertEquals(products);
        }

        [TestCase]
        public void Success_OneProduct()
        {
            IEnumerable<ProductId>? products = Bridge.SearchProducts(new ProductSearchCreteria("charger")
            {
                PriceRange_High = 21
            });
            new Assert_SetEquals<ProductId>("Products search - success one shop", Product2_2)
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
    }
}
