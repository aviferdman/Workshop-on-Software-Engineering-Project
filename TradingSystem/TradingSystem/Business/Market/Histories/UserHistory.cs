using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class UserHistory : IHistory
    {
        ICollection<TransactionStatus> transactionStatuses;
        public UserHistory()
        {
            TransactionStatuses = new HashSet<TransactionStatus>();
        }

        public ICollection<TransactionStatus> TransactionStatuses { get => transactionStatuses; set => transactionStatuses = value; }

        public void Add(TransactionStatus t)
        {
            TransactionStatuses.Add(t);
        }

        public ICollection<DeliveryStatus> GetDeliveryStatuses()
        {
            return TransactionStatuses.Select(t => t.DeliveryStatus).ToList();
        }

        public ICollection<PaymentStatus> GetPaymentStatuses()
        {
            return TransactionStatuses.Select(t => t.PaymentStatus).ToList();
        }

        public ICollection<ProductHistoryData> GetProductsStatuses()
        {
            return TransactionStatuses.Select(t => t.ProductHistories).ToList();
        }

        public void AddHistory(ICollection<PurchaseStatus> purchaseStatuses)
        {
            foreach (var p in purchaseStatuses)
            {
                transactionStatuses.Add(p.TransactionStatus);
            }
        }
    }
}
