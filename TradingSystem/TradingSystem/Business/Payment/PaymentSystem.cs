using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Payment
{
    public class PaymentSystem : ExternalPaymentSystem
    {
        private static readonly Lazy<PaymentSystem>
        _lazy =
        new Lazy<PaymentSystem>
            (() => new PaymentSystem());

        public static PaymentSystem Instance { get { return _lazy.Value; } }

        private PaymentSystem()
        {
        }

        public Guid CancelPayment(Guid paymentId)
        {
            return generatePaymentId();
        }

        public Guid generatePaymentId()
        {
            return Guid.NewGuid();
        }

        public Guid CreatePayment(string username, string paymentMethod, int accountNumber2, int branch2, double paymentSum)
        {
            return Guid.NewGuid();
        }
    }
}
