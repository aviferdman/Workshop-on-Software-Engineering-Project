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
    /// Use case 24: remove product from shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/19
    [TestFixture(SHOP_NAME, USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD)]
    public class UseCase_RemoveProduct : ShopManagementTestBase
    {
        private UseCase_AddProduct useCase_addProduct;
        private ProductId product;

        public UseCase_RemoveProduct(string shopName, string shopUsername, string shopUserPassword) :
            this(shopName, SystemContext.Instance, new UserInfo(shopUsername, shopUserPassword))
        { }
        public UseCase_RemoveProduct(string shopName, SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        {
            ShopName = shopName;
        }

        public string ShopName { get; }
        public ShopId ShopId => useCase_addProduct.ShopId;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_addProduct = new UseCase_AddProduct(ShopName, SystemContext.Instance, UserInfo);
            useCase_addProduct.Setup();
            product = useCase_addProduct.Success_Normal(new ProductInfo("cucumber", 4, 7));
        }

        [TearDown]
        public override void Teardown()
        {
            useCase_addProduct.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.RemoveProductFromShop(ShopId, product));
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsFalse(Bridge.RemoveProductFromShop(ShopId, product));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            Assert.IsFalse(Bridge.RemoveProductFromShop(ShopId, new ProductId(int.MaxValue - 1)));
        }
    }
}
