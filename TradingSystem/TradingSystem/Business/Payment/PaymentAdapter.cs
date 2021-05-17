using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    public interface PaymentAdapter
    {
        public bool SetPaymentSystem(ExternalPaymentSystem externalPaymentSystem);
        public Task<PaymentStatus> CreatePayment(PaymentDetails paymentDetails);
        public Task<PaymentStatus> CancelPayment(PaymentStatus paymentStatus);
    }
}
