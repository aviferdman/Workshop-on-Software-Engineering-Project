﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class UserMangementIntegrationTest
    {
        public UserMangementIntegrationTest()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        private async Task<string> signupAsync()
        {
            await UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la", "1111111"), "0501234733");
            return MarketUsers.Instance.AddGuest();
        }

        private async Task deleteAsync(string username)
        {
            MemberState m;
            await MarketUsers.Instance.logout(username);
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.tearDown();
        }
        private void delete2(string username)
        {
            MemberState m;
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.tearDown();
        }


        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestIntegLoginSuccess()
        {
            string user=await signupAsync();
            User u;
            Assert.AreEqual("success", MarketUsers.Instance.AddMember("inbi2001", "123456", user));
            Assert.IsTrue(MarketUsers.Instance.ActiveUsers.TryGetValue("inbi2001", out u));
            Assert.IsInstanceOfType(u.State, typeof(MemberState));
            await deleteAsync("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// already logged in
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestIntegLoginFailed1()
        {
            string user = await signupAsync();
            await MarketUsers.Instance.AddMember("inbi2001", "123456", user);
            Assert.AreEqual("user is already logged in", MarketUsers.Instance.AddMember("inbi2001", "123456", user));
            await deleteAsync ("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// password doesn't match username
        [TestMethod]
        [TestCategory("uc2")]
        public async void TestIntegLoginFailed2()
        {
            string user =await signupAsync();
            Assert.AreEqual("the password doesn't match username: " + "inbi2001", MarketUsers.Instance.AddMember("inbi2001", "12345d6", user));
            Assert.IsFalse(MarketUsers.Instance.ActiveUsers.ContainsKey("inbi2001"));
            await deleteAsync ("inbi2001");

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.AddMember(string, string, string)"/>
        /// username doesn't exist
        [TestMethod]
        [TestCategory("uc2")]
        public void TestIntegLoginFailed3()
        {
            Assert.AreEqual("username: " + "inbi2001" + " doesn't exist in the system", MarketUsers.Instance.AddMember("inbi2001", "12345d6","lilk"));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        [TestMethod]
        [TestCategory("uc3")]
        public async void TestIntegLogoutSuccess()
        {
            string user = await signupAsync();
            await MarketUsers.Instance.AddMember("inbi2001", "123456", user);
            Assert.AreNotEqual(null, MarketUsers.Instance.logout("inbi2001"));
            Assert.IsFalse(MarketUsers.Instance.ActiveUsers.ContainsKey("inbi2001"));
            await deleteAsync(user);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        /// not logged in
        [TestMethod]
        [TestCategory("uc3")]
        public async void TestIntegLogoutFail1()
        {
            string user = await signupAsync();
            Assert.AreEqual(null, MarketUsers.Instance.logout("inbi2001"));
            delete2(user);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketUsers.logout(string)"/>
        /// user doesn't exist
        [TestMethod]
        [TestCategory("uc3")]
        public void TestIntegLogoutFail2()
        {
            Assert.AreEqual(null, MarketUsers.Instance.logout("inbi200151"));
        }

    }
}
