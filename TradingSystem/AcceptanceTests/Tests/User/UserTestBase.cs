using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.UserBridge;

using NUnit.Framework;

namespace AcceptanceTests.Tests.User
{
    public class UserTestBase
    {
        protected SystemContext SystemContext { get; }

        public UserTestBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        protected IUserBridge Bridge => SystemContext.UserBridge;
    }
}
