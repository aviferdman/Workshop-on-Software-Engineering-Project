using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Delivery
{
    class PaymentImpl : PaymentAdapter
    {
        private PaymentSystem _paymentSystem;        
        
        public PaymentImpl()
        {
            this._paymentSystem = PaymentSystem.Instance;
        }

        public Task<bool> CancelPayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            return _paymentSystem.CancelPayment(clientId, clientBankAccountId, recieverBankAccountId, paymentSum);
        }

        public Task<bool> CreatePayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            return _paymentSystem.CreatePayment(clientId, clientBankAccountId, recieverBankAccountId, paymentSum);
        }

    }
}
