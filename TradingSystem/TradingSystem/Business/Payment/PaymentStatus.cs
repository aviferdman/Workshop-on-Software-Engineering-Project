using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    class PaymentStatus
    {
        string _paymentId;
        bool _status;

        public PaymentStatus(string paymentId, bool status)
        {
            this.PaymentId = paymentId;
            this.Status = status;
        }

        public string PaymentId { get => _paymentId; set => _paymentId = value; }
        public bool Status { get => _status; set => _status = value; }
    }
}
