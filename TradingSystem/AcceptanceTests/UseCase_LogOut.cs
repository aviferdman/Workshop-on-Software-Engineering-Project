using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.User;

using NUnit.Framework;

namespace AcceptanceTests
{
    /// <summary>
    /// Acceptance test for
    /// Use case 3: Log out
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/51
    /// </summary>
    [TestFixture("logout_user123", "mypassword1")]
    public class UseCase_LogOut
    {
        private readonly string username;
        private readonly string password;

        private IUserBridge bridge;
        private UseCase_Login test_login;

        public UseCase_LogOut(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace.", nameof(username));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));
            }

            this.username = username;
            this.password = password;
        }

        [SetUp]
        public void Setup()
        {
            bridge = Driver.UserBridge();
            test_login = new UseCase_Login(username, password);
            test_login.Setup();
            test_login.Success_Normal();
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, bridge.LogOut());
        }
    }
}
