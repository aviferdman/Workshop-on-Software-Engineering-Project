using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    class PaymentDetails
    {
        private Guid _clientId;
        private Guid _clientBankAccountId;
        private Guid _recieverBankAccountId;
        private double _paymentSum;

        public PaymentDetails(Guid clientId, Guid clientBankAccountId, Guid recieverBankAccountId, double paymentSum)
        {
            this.ClientId = clientId;
            this.ClientBankAccountId = clientBankAccountId;
            this.RecieverBankAccountId = recieverBankAccountId;
            this.PaymentSum = paymentSum;
        }

        public Guid ClientId { get => _clientId; set => _clientId = value; }
        public Guid ClientBankAccountId { get => _clientBankAccountId; set => _clientBankAccountId = value; }
        public Guid RecieverBankAccountId { get => _recieverBankAccountId; set => _recieverBankAccountId = value; }
        public double PaymentSum { get => _paymentSum; set => _paymentSum = value; }
    }
}
