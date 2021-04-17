﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class DeliveryTests
    {

        /// test for function :<see cref="TradingSystem.Business.Delivery.DeliveryAdapter.CreateDelivery(DeliveryDetails)"/>
        [TestMethod]
        public void LegalDelivery()
        {
            Guid clientId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Address source = new Address("1", "1", "1", "1");
            Address destination = new Address("2", "2", "2", "2");
            DeliveryAdapter deliveryAdapter = new DeliveryImpl();
            DeliveryDetails deliveryDetails = new DeliveryDetails(clientId, storeId, "0544444444", 1, source, destination);
            DeliveryStatus deliveryStatus = deliveryAdapter.CreateDelivery(deliveryDetails);
            Assert.AreEqual(true, deliveryStatus.Status);
        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

    }
}
