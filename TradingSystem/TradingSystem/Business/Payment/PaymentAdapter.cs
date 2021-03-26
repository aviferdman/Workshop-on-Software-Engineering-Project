using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    interface PaymentAdapter
    {
        public Task<bool> CreatePayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum);
        public Task<bool> CancelPayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum);
    }
}
