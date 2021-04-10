﻿
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public class UseCase_AddProductToCart_TestLogic : MarketTestBase
    {
        public UseCase_AddProductToCart_TestLogic(SystemContext systemContext) :
            base(systemContext)
        { }

        public void Success_Normal(ProductInCart product)
        {
            Assert.IsTrue(Bridge.AddProductToUserCart(product));
        }
    }
}