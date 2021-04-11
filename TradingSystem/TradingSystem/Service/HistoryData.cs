using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Service
{
    public class HistoryData
    {
        ICollection<DeliveryStatus> _deliveries;
        ICollection<PaymentStatus> _payments;
        ICollection<ProductsStatus> _products;
        public HistoryData(History history)
        {
            this._deliveries = history.Deliveries;
            this._payments = history.Payments;
            this._products = history.Products;
        }
    }
}
