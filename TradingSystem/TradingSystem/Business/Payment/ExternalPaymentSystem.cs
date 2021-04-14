using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Payment
{
    public interface ExternalPaymentSystem
    {
        public Guid CancelPayment(Guid paymentId);

        public Guid generatePaymentId();

        public Guid CreatePayment(Guid clientId, string paymentMethod, int accountNumber2, int branch2, double paymentSum);
    }
}
