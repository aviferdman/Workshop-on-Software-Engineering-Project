using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    interface PaymentAdapter
    {
        public PaymentStatus CreatePayment(PaymentDetails paymentDetails);
        public PaymentStatus CancelPayment(PaymentDetails paymentDetails);
    }
}
