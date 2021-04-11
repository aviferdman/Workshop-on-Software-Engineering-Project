
using AcceptanceTests.AppInterface;

using NUnit.Framework;

namespace AcceptanceTests.Tests.User
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
