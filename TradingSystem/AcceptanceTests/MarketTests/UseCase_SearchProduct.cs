using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 4: Search product
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/52
    /// </summary>
    [TestFixture(USER_BUYER_NAME, USER_BUYER_PASSWORD)]
    public class UseCase_SearchProduct : MarketTestBase
    {
        private UseCase_AddProduct useCase_addProduct_shop1;
        private Product product1;

        private UseCase_AddProduct useCase_addProduct_shop2;
        private Product product2;

        public UseCase_SearchProduct(SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        { }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            product1 = AddProduct(
                SHOP_NAME,
                USER_SHOP_OWNER_NAME,
                USER_SHOP_OWNER_PASSWORD,
                new ProductInfo("traveling bag", 20, 7),
                out useCase_addProduct_shop1
            );
            new UseCase_LogOut(SystemContext, new UserInfo("h", "h"))
                .Success_Normal();
            product2 = AddProduct(
                SHOP_NAME_2,
                USER_SHOP_OWNER_2_NAME,
                USER_SHOP_OWNER_2_PASSWORD,
                new ProductInfo("school bag", 5, 12),
                out useCase_addProduct_shop2
            );
        }

        [TearDown]
        public void Teardown()
        {
            useCase_addProduct_shop2.Teardown();
            _ = Login(new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD));
            useCase_addProduct_shop1.Teardown();
        }

        private Product AddProduct(string shopName, string shopUsername, string shopUserPassword, ProductInfo productInfo, out UseCase_AddProduct useCase_addProduct)
        {
            useCase_addProduct = new UseCase_AddProduct(shopName, SystemContext, new UserInfo(shopUsername, shopUserPassword));
            useCase_addProduct.Setup();
            return useCase_addProduct.Success_Normal(productInfo);
        }

        [TestCase]
        public void Success_Normal()
        {
            IEnumerable<Product>? products = Bridge.SearchProducts(new ProductSearchCreteria("bag")
            {
                PriceRange_Low = 0.5m
            });
            Assert.NotNull(products, "Products search - success - null results");
            Assert.IsNotEmpty(products, "Products search - success - empty results");
            using IEnumerator<Product> enumerator = products!.GetEnumerator();
            _ = enumerator.MoveNext();
            if (product1.Equals(enumerator.Current))
            {
                Assert.IsTrue(enumerator.MoveNext(), "Products search - success - expected more than 1 result");
                Assert.AreEqual(product2, enumerator.Current, $"One of the result products must be {product2}");
            }
            else
            {
                Assert.AreEqual(product2, enumerator.Current, $"One of the result products must be {product2}");
                Assert.IsTrue(enumerator.MoveNext(), "Products search - success - expected more than 1 result");
                Assert.AreEqual(product1, enumerator.Current, $"One of the result products must be {product1}");
            }
            Assert.IsFalse(enumerator.MoveNext(), "Products search - success - expected exactly 2 result");
        }
    }
}
