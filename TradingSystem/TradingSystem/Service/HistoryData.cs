using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Service
{
    public class HistoryData
    {
        DeliveryStatus _deliveries;
        PaymentStatus _payments;
        ProductHistoryData _products;
        public HistoryData(IHistory history)
        {
            this._deliveries = history.GetDeliveryStatus();
            this._payments = history.GetPaymentStatus();
            this._products = history.GetProductsStatus();
        }
    }
}
