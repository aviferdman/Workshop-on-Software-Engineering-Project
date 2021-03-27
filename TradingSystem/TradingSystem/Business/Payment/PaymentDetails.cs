using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    class PaymentDetails
    {
        private string _clientId;
        private string _clientBankAccountId;
        private string _recieverBankAccountId;
        private double _paymentSum;

        public PaymentDetails(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            this.ClientId = clientId;
            this.ClientBankAccountId = clientBankAccountId;
            this.RecieverBankAccountId = recieverBankAccountId;
            this.PaymentSum = paymentSum;
        }

        public string ClientId { get => _clientId; set => _clientId = value; }
        public string ClientBankAccountId { get => _clientBankAccountId; set => _clientBankAccountId = value; }
        public string RecieverBankAccountId { get => _recieverBankAccountId; set => _recieverBankAccountId = value; }
        public double PaymentSum { get => _paymentSum; set => _paymentSum = value; }
    }
}
