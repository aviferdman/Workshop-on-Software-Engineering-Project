using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Service;

namespace AcceptanceTests
{
    public class TestInit
    {
        [Test]
        public void goodInit()
        {
            Assert.IsFalse(SetupService.Instance.Initialize().Result.IsErr);
        }
    }
}
