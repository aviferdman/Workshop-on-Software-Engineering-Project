using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    public class PaymentStatus
    {
        Guid _paymentId;
        string _username;
        Guid _storeId;
        bool _status;

        public PaymentStatus(Guid paymentId, string username, Guid storeId, bool status)
        {
            this.PaymentId = paymentId;
            this.Username = username;
            this._storeId = storeId;
            this.Status = status;
        }

        public Guid PaymentId { get => _paymentId; set => _paymentId = value; }
        public bool Status { get => _status; set => _status = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public string Username { get => _username; set => _username = value; }
    }
}
