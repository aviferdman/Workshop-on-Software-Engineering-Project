using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.User;

using NUnit.Framework;

namespace AcceptanceTests
{
    public class MemberUseTestBase
    {
        public const string USERNAME = "user123";
        public const string PASSWORD = "mypassword1";
        protected IUserBridge Bridge { get; }

        public MemberUseTestBase()
        {
            Bridge = Driver.UserBridge;
        }

        [SetUp]
        public virtual void Setup() { }
    }
}
