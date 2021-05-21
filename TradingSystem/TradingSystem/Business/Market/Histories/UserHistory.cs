using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market.Histories
{
    public class UserHistory : IHistory
    {
        public TransactionStatus _transactionStatus { get; set; }
        public UserHistory(TransactionStatus transactionStatus)
        {
            _transactionStatus = transactionStatus;
        }

        public UserHistory(PurchaseStatus p)
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
