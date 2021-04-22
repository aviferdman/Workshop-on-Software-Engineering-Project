using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market.Histories
{
    public class StoreHistory : IHistory
    {
        private TransactionStatus _transactionStatus;
        public StoreHistory(TransactionStatus transactionStatus)
        {
            _transactionStatus = transactionStatus;
        }

        public StoreHistory(PurchaseStatus p)
        {
            _transactionStatus = p.TransactionStatus;
        }

        public DeliveryStatus GetDeliveryStatus()
        {
            return _transactionStatus.DeliveryStatus;
        }

        public PaymentStatus GetPaymentStatus()
        {
            return _transactionStatus.PaymentStatus;
        }

        public ProductHistoryData GetProductsStatus()
        {
            return _transactionStatus.ProductHistories;
        }
    }
}
