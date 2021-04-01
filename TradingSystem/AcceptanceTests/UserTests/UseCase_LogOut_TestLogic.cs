using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.UserTests;

using NUnit.Framework;

namespace AcceptanceTests.UserTests
{
    public class UseCase_LogOut_TestLogic : UserTestBase
    {
        public UseCase_LogOut_TestLogic(SystemContext systemContext) :
            base(systemContext)
        { }

        public void Success_Normal()
        {
            Assert.AreEqual(true, Bridge.LogOut());
        }
    }
}
