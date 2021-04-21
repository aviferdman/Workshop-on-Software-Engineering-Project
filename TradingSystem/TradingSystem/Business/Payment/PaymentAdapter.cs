using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    public interface PaymentAdapter
    {
        public bool SetPaymentSystem(ExternalPaymentSystem externalPaymentSystem);
        public PaymentStatus CreatePayment(PaymentDetails paymentDetails);
        public PaymentStatus CancelPayment(PaymentStatus paymentStatus);
    }
}
