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
    /// Use case 23: add product to shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/18
    [TestFixture(SHOP_NAME, USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD)]
    public class UseCase_AddProduct : ShopManagementTestBase
    {
        private UseCase_OpenShop useCase_openShop;
        private Shop shop;
        private Product product;

        public UseCase_AddProduct(string shopName, string shopUsername, string shopUserPassword) :
            this(shopName, SystemContext.Instance, new UserInfo(shopUsername, shopUserPassword))
        { }
        public UseCase_AddProduct(string shopName, SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        {
            ShopName = shopName;
        }

        public string ShopName { get; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_openShop = new UseCase_OpenShop(SystemContext.Instance, UserInfo);
            useCase_openShop.Setup();
            shop = useCase_openShop.Success_Normal(new ShopInfo(ShopName));
        }

        [TearDown]
        public void Teardown()
        {
            _ = Bridge.RemoveProduct(shop, product);
            useCase_openShop.Teardown();
        }

        [TestCase("cucumber", 4, 4)]
        public void Success_Normal_Test(string productName, int quantity, int price)
        {
            product = Success_Normal(new ProductInfo(productName, quantity, price));
        }
        public Product Success_Normal(ProductInfo productInfo)
        {
            Product? product = Bridge.AddProduct(shop, productInfo);
            Assert.IsNotNull(product);
            Assert.AreEqual(productInfo.Name, product!.Name);
            Assert.AreEqual(productInfo.Quantity, product!.Quantity);
            Assert.AreEqual(productInfo.Price, product!.Price);
            Assert.Greater(product!.Id, 0);
            return product;
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            var useCase_login = new UseCase_Login(SystemContext, new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD));
            useCase_login.Setup();
            useCase_login.Success_Normal();

            Assert.IsNull(Bridge.AddProduct(shop, new ProductInfo("cucumber", 4, 3)));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsNull(Bridge.AddProduct(shop, new ProductInfo("cucumber", -3, 3)));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsNull(Bridge.AddProduct(shop, new ProductInfo("", 4, 3)));
        }
    }
}
