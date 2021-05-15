using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using System.Threading;
using System.Threading.Tasks;

namespace TradingSystemTests.IntegrationTests.ConcurrentTests
{
    [TestClass]
    public class AddAppointmentConcurrecy
    {/*
        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentSignup()
        {
            String val1 = "";
            String val2 = "";
            Task task1 = Task.Factory.StartNew(() => val1 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
            Task task2 = Task.Factory.StartNew(() => val2 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
            Task.WaitAll(task1, task2);
            bool check1 = String.Equals("success", val1);
            bool check2 = String.Equals("success", val2);

            Assert.IsTrue(check1 || check2);
            Assert.IsFalse(check1 && check2);
            delete2("inbi2001");
        }*/
    }
}
