﻿using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
{
    public class UseCase_AddProductToCart_TestLogic : MarketTestBase
    {
        public UseCase_AddProductToCart_TestLogic(SystemContext systemContext) :
            base(systemContext)
        { }

        public void Success_Normal(ProductId productId)
        {
            Assert.IsTrue(Bridge.AddProductToUserCart(productId));
        }
    }
}
