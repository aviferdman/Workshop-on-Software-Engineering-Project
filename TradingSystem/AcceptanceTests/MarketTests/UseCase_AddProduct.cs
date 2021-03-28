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
        private Product? product;

        public UseCase_AddProduct(string shopName, string shopUsername, string shopUserPassword) :
            this(shopName, SystemContext.Instance, new UserInfo(shopUsername, shopUserPassword))
        { }
        public UseCase_AddProduct(string shopName, SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        {
            ShopName = shopName;
        }

        public string ShopName { get; }
        public Shop Shop { get; private set; }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_openShop = new UseCase_OpenShop(SystemContext, UserInfo);
            useCase_openShop.Setup();
            Shop = useCase_openShop.Success_Normal(new ShopInfo(ShopName));
        }

        [TearDown]
        public void Teardown()
        {
            if (product != null)
            {
                _ = Bridge.RemoveProduct(Shop, product);
                product = null;
            }
            useCase_openShop.Teardown();
        }

        [TestCase("cucumber", 4, 4)]
        public void Success_Normal_Test(string productName, int quantity, int price)
        {
            _ = Success_Normal(new ProductInfo(productName, quantity, price));
        }
        public Product Success_Normal(ProductInfo productInfo)
        {
            Product? product = Bridge.AddProduct(Shop, productInfo);
            Assert.IsNotNull(product);
            Assert.AreEqual(productInfo.Name, product!.Name);
            Assert.AreEqual(productInfo.Quantity, product!.Quantity);
            Assert.AreEqual(productInfo.Price, product!.Price);
            Assert.Greater(product!.Id, 0);
            this.product = product;
            return product;
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsNull(Bridge.AddProduct(Shop, new ProductInfo("cucumber", 4, 3)));
        }

        [TestCase]
        public void Failure_InvalidPrice()
        {
            Assert.IsNull(Bridge.AddProduct(Shop, new ProductInfo("cucumber", -3, 3)));
        }

        [TestCase]
        public void Failure_InvalidName()
        {
            Assert.IsNull(Bridge.AddProduct(Shop, new ProductInfo("", 4, 3)));
        }
    }
}
