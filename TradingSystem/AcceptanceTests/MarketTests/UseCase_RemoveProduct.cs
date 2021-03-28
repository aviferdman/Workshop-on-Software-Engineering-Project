﻿using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 24: add product to shop
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/19
    [TestFixture(SHOP_NAME, USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD)]
    public class UseCase_RemoveProduct : ShopManagementTestBase
    {
        private UseCase_AddProduct useCase_addProduct;
        private Product product;

        public UseCase_RemoveProduct(string shopName, string shopUsername, string shopUserPassword) :
            this(shopName, SystemContext.Instance, new UserInfo(shopUsername, shopUserPassword))
        { }
        public UseCase_RemoveProduct(string shopName, SystemContext systemContext, UserInfo userInfo) :
            base(systemContext, userInfo)
        {
            ShopName = shopName;
        }

        public string ShopName { get; }
        public Shop Shop => useCase_addProduct.Shop;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            useCase_addProduct = new UseCase_AddProduct(ShopName, SystemContext.Instance, UserInfo);
            useCase_addProduct.Setup();
            product = useCase_addProduct.Success_Normal(new ProductInfo("cucumber", 4, 7));
        }

        [TearDown]
        public void Teardown()
        {
            useCase_addProduct.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.RemoveProduct(Shop, product));
        }

        [TestCase]
        public void Failure_InsufficientPermissions()
        {
            LoginToBuyer();
            Assert.IsFalse(Bridge.RemoveProduct(Shop, product));
        }

        [TestCase]
        public void Failure_ProductDoesNotExist()
        {
            Assert.IsFalse(Bridge.RemoveProduct(Shop, new Product(product.Name, product.Price, product.Quantity, int.MaxValue - 1)));
        }
    }
}
