using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    class History
    {
        ICollection<DeliveryStatus> _deliveries;
        ICollection<PaymentStatus> _payments;

        public History()
        {
            this._deliveries = new HashSet<DeliveryStatus>();
            this._payments = new HashSet<PaymentStatus>();
        }

        private History(IEnumerable<DeliveryStatus> deliveries, IEnumerable<PaymentStatus> payments)
        {
            this._deliveries = deliveries.ToList();
            this._payments = payments.ToList();
        }

        public void AddDelivery(DeliveryStatus deliveryStatus)
        {
            _deliveries.Add(deliveryStatus);
        }

        public void AddPayment(PaymentStatus paymentStatus)
        {
            _payments.Add(paymentStatus);
        }

        public History GetHistory(Guid userId)
        {
            var deliveries = _deliveries.Where( d => d.ClientId.Equals(userId));
            var payments = _payments.Where( p => p.ClientId.Equals(userId));
            return new History(deliveries, payments);
        }

        internal History GetStoreHistory(Guid storeId)
        {
            var deliveries = _deliveries.Where(d => d.StoreId.Equals(storeId));
            var payments = _payments.Where(p => p.StoreId.Equals(storeId));
            return new History(deliveries, payments);
        }
    }
}
