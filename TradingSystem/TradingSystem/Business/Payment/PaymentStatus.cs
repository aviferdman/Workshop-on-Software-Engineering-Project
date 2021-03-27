using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    class PaymentStatus
    {
        Guid _paymentId;
        bool _status;

        public PaymentStatus(Guid paymentId, bool status)
        {
            this.PaymentId = paymentId;
            this.Status = status;
        }

        public Guid PaymentId { get => _paymentId; set => _paymentId = value; }
        public bool Status { get => _status; set => _status = value; }
    }
}
