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
        public HistoryData(IHistory history)
        {
            this.Deliveries = history.GetDeliveryStatus();
            this.Payments = history.GetPaymentStatus();
            this.Products = history.GetProductsStatus();
        }

        public DeliveryStatus Deliveries { get; }
        public PaymentStatus Payments { get; }
        public ProductHistoryData Products { get; }
    }
}
