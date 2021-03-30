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
        private bool _status;

        public TransactionStatus(PaymentStatus paymentStatus, DeliveryStatus deliveryStatus, bool status)
        {
            this._paymentStatus = paymentStatus;
            this._deliveryStatus = deliveryStatus;
            this.Status = status;
            this._id = new Guid();
        }
        public Guid Id { get => _id; set => _id = value; }
        public bool Status { get => _status; set => _status = value; }
        internal DeliveryStatus DeliveryStatus { get => _deliveryStatus; set => _deliveryStatus = value; }
        internal PaymentStatus PaymentStatus { get => _paymentStatus; set => _paymentStatus = value; }
    }
}
