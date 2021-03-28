using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    class PaymentStatus
    {
        Guid _paymentId;
        Guid _clientId;
        Guid _storeId;
        bool _status;

        public PaymentStatus(Guid paymentId, Guid clientId, Guid storeId, bool status)
        {
            this.PaymentId = paymentId;
            this._clientId = clientId;
            this._storeId = storeId;
            this.Status = status;
        }

        public Guid PaymentId { get => _paymentId; set => _paymentId = value; }
        public bool Status { get => _status; set => _status = value; }
        public Guid ClientId { get => _clientId; set => _clientId = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
    }
}
