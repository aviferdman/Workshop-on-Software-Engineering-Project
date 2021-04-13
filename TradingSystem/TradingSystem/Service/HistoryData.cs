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
        ICollection<DeliveryStatus> _deliveries;
        ICollection<PaymentStatus> _payments;
        ICollection<ProductHistoryData> _products;
        public HistoryData(IHistory history)
        {
            this._deliveries = history.GetDeliveryStatuses();
            this._payments = history.GetPaymentStatuses();
            this._products = history.GetProductsStatuses();
        }
    }
}
