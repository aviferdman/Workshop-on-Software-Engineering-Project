using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Delivery
{
    class PaymentImpl : PaymentAdapter
    {
        private readonly string ErrorPaymentId = "";
        private PaymentSystem _paymentSystem;        
        
        public PaymentImpl()
        {
            this._paymentSystem = PaymentSystem.Instance;
        }

        public PaymentStatus CancelPayment(PaymentDetails paymentDetails)
        {
            Guid paymentId = _paymentSystem.CancelPayment(paymentDetails.ClientId, paymentDetails.ClientBankAccountId, paymentDetails.RecieverBankAccountId, paymentDetails.PaymentSum);
            return new PaymentStatus(paymentId, !paymentId.ToString().Equals(ErrorPaymentId));
        }

        public PaymentStatus CreatePayment(PaymentDetails paymentDetails)
        {
            Guid paymentId = _paymentSystem.CreatePayment(paymentDetails.ClientId, paymentDetails.ClientBankAccountId, paymentDetails.RecieverBankAccountId, paymentDetails.PaymentSum);
            return new PaymentStatus(paymentId, !paymentId.ToString().Equals(ErrorPaymentId));
        }

    }
}
