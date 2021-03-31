using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Delivery
{
    public class PaymentSystem
    {
        private static readonly Lazy<PaymentSystem>
        _lazy =
        new Lazy<PaymentSystem>
            (() => new PaymentSystem());

        public static PaymentSystem Instance { get { return _lazy.Value; } }

        private PaymentSystem()
        {
        }

        public Guid CreatePayment(Guid clientId, BankAccount clientBankAccountId, BankAccount recieverBankAccountId, double paymentSum)
        {
            return generatePaymentId();
        }

        public Guid CancelPayment(Guid paymentId)
        {
            return generatePaymentId();
        }

        public Guid generatePaymentId()
        {
            return Guid.NewGuid();
        }
    }
}
