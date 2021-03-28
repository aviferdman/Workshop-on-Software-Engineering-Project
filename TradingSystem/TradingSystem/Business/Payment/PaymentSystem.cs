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

        public Guid CreatePayment(Guid clientId, Guid clientBankAccountId, Guid recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        public Guid CancelPayment(Guid paymentId)
        {
            return generatePaymentId();
        }

        public Guid generatePaymentId()
        {
            return new Guid();
        }
    }
}
