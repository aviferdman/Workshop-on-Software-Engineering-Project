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
        public void Add(TransactionStatus t);

        public ICollection<DeliveryStatus> GetDeliveryStatuses();

        public ICollection<PaymentStatus> GetPaymentStatuses();

        public ICollection<ProductHistoryData> GetProductsStatuses();
    }
}
