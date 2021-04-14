using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Interfaces
{
    public interface IHistory
    {

        public DeliveryStatus GetDeliveryStatus();

        public PaymentStatus GetPaymentStatus();

        public ProductHistoryData GetProductsStatus();
    }
}
