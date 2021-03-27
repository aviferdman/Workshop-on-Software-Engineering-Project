using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.UserBridge;

using NUnit.Framework;

namespace AcceptanceTests
{
    public class MemberUseTestBase
    {
        public const string USERNAME = "user123";
        public const string PASSWORD = "mypassword1";

        protected IUserBridge Bridge { get; }
        protected SystemContext SystemContext { get; }
        public UserInfo UserInfo { get; }

        public MemberUseTestBase(SystemContext systemContext, UserInfo loginInfo)
        {
            SystemContext = systemContext;
            Bridge = systemContext.UserBridge;
            UserInfo = loginInfo;
        }

        [SetUp]
        public virtual void Setup() { }
    }
}
