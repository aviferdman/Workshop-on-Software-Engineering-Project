﻿using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Service
{
    public class MarketGeneralService
    {
        private readonly MarketUsers marketUsers;
        private readonly MarketStores marketStores;

        public MarketGeneralService()
        {
            marketUsers = MarketUsers.Instance;
            marketStores = MarketStores.Instance;
        }

        public void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode = false)
        {
            marketStores.ActivateDebugMode(deliveryAdapter, paymentAdapter, debugMode);
        }

        public ICollection<HistoryData> GetAllHistory(string username)
        {
            ICollection<IHistory> histories = marketUsers.GetAllHistory(username);
            ICollection<HistoryData> ret = new HashSet<HistoryData>();
            foreach (IHistory his in histories)
            {
                ret.Add(new HistoryData(his));
            }
            return ret;
        }
    }
}
