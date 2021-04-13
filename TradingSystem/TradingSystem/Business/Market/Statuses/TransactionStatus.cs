using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class TransactionStatus
    {
        private Guid _id;
        private PaymentStatus _paymentStatus;
        private DeliveryStatus _deliveryStatus;
        private ProductHistoryData productHistories;
        private bool _status;

        public TransactionStatus(PaymentStatus paymentStatus, DeliveryStatus deliveryStatus, IShoppingBasket shoppingBasket, bool status)
        {
            this._paymentStatus = paymentStatus;
            this._deliveryStatus = deliveryStatus;
            this.Status = status;
            this._id = Guid.NewGuid();
            this.ProductHistories = new ProductHistoryData();
            var tmpDict = shoppingBasket.GetDictionaryProductQuantity();
            foreach(var p_q in tmpDict)
            {
                ProductHistories.Add(p_q.Key, p_q.Value);
            }
        }
        public Guid Id { get => _id; set => _id = value; }
        public bool Status { get => _status; set => _status = value; }
        public DeliveryStatus DeliveryStatus { get => _deliveryStatus; set => _deliveryStatus = value; }
        public PaymentStatus PaymentStatus { get => _paymentStatus; set => _paymentStatus = value; }
        public ProductHistoryData ProductHistories { get => productHistories; set => productHistories = value; }
    }
}
