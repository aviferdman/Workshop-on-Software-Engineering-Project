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

        internal Guid CreatePayment(Guid clientId, Guid clientBankAccountId, Guid recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        internal Guid CancelPayment(Guid clientId, Guid clientBankAccountId, Guid recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        private Guid generatePaymentId()
        {
            return new Guid();
        }
    }
}
