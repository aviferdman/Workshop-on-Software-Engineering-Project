using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;
using TradingSystem.Service;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class LoggerTests
    {
        private Logger logger;
        private MarketUsers marketUsers;

        public LoggerTests()
        {
            logger = Logger.Instance;
            marketUsers = MarketUsers.Instance;
            logger.CleanLogs();
        }

        /// test for function :<see cref="TradingSystem.Business.Logger.MonitorActivity(string)"/>
        [TestMethod]
        public void SuccessfulyRecordActivity()
        {
            Assert.AreEqual(0, logger.Activities.Count);
            marketUsers.RemoveGuest("testguest");
            Assert.AreEqual(1, logger.Activities.Count);
            Assert.AreEqual("MarketUsers RemoveGuest", logger.Activities[0]);
        }

        /// test for function :<see cref="TradingSystem.Business.Logger.MonitorError(string)"/>
        [TestMethod]
        public void RecordError()
        {
            MarketGeneralService marketGeneralService = MarketGeneralService.Instance;
            Mock<PaymentAdapter> paymentAdapter = new Mock<PaymentAdapter>();
            paymentAdapter.Setup(p => p.SetPaymentSystem(It.IsAny<ExternalPaymentSystem>())).Returns(false);
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter = paymentAdapter.Object;
            Assert.AreEqual(0, logger.Errors.Count);
            marketGeneralService.ActivateDebugMode(new Mock<ExternalDeliverySystem>(), new Mock<ExternalPaymentSystem>(), true);
            Assert.AreEqual(1, logger.Errors.Count);
            Assert.AreEqual("Error: Transaction ActivateDebugMode", logger.Errors[0]);

        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
            logger.CleanLogs();
        }
    }
}
