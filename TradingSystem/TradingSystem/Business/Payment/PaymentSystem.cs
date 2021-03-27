using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    class PaymentSystem
    {
        private static readonly Lazy<PaymentSystem>
        _lazy =
        new Lazy<PaymentSystem>
            (() => new PaymentSystem());

        public static PaymentSystem Instance { get { return _lazy.Value; } }

        private PaymentSystem()
        {
        }

        internal string CreatePayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        internal string CancelPayment(string clientId, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        private string generatePaymentId()
        {
            return new Guid().ToString();
        }
    }
}
