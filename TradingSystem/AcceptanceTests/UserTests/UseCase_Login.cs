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
    [TestFixture(USERNAME, PASSWORD)]
    public class UseCase_Login : MemberUseTestBase
    {
        private readonly string username;
        private readonly string password;

        private UseCase_SignUp test_signUp;

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
        public override void Setup()
        {
            base.Setup();
            test_signUp = new UseCase_SignUp(username, password);
            test_signUp.Setup();
            test_signUp.Success_Normal();
        }

        [Test]
        public void Success_Normal()
        {
            Assert.AreEqual(true, Bridge.Login(username, password));
        }

        [Test]
        public void Failure_PasswordMismatch()
        {
            Assert.AreEqual(false, Bridge.Login(username, "wrongpasswrod"));
        }

        [Test]
        public void Failure_UsernameDoesntExist()
        {
            Assert.AreEqual(false, Bridge.Login("invaliduser1", "wrongpasswrod"));
        }

        [Test]
        public void Failure_AlreadyLoggedIn()
        {
            Success_Normal();
            Assert.AreEqual(false, Bridge.Login(username, password));
        }
    }
}
