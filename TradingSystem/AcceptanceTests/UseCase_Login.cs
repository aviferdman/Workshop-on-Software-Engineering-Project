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
    /// Use case 2: Login
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21
    /// </summary>
    public class UseCase_Login
    {
        private readonly string username;
        private readonly string password;

        private IUserBridge bridge;
        private UseCase_SignUp test_signUp;

        public UseCase_Login() : this("user123", "hi1there2") { }
        public UseCase_Login(string username, string password)
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
            test_signUp = new UseCase_SignUp();
            test_signUp.Setup();
            test_signUp.Success_Normal(username, password);
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, bridge.Login(username, password));
        }

        [Test]
        public void Failure_PasswordMismatch()
        {
            Assert.AreEqual(false, bridge.Login(username, "wrongpasswrod"));
        }

        [Test]
        public void Failure_UsernameDoesntExist()
        {
            Assert.AreEqual(false, bridge.Login("invaliduser1", "wrongpasswrod"));
        }

        [Test]
        public void Failure_AlreadyLoggedIn()
        {
            Success_Normal();
            Assert.AreEqual(false, bridge.Login(username, password));
        }
    }
}
