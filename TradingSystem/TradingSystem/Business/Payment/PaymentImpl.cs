using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Delivery
{
    public class PaymentImpl : PaymentAdapter
    {
        private readonly string ErrorPaymentId = "";
        private PaymentSystem _paymentSystem;        
        
        public PaymentImpl()
        {
            this._paymentSystem = PaymentSystem.Instance;
        }

        public PaymentStatus CancelPayment(PaymentStatus paymentStatus)
        {
            Guid paymentId = _paymentSystem.CancelPayment(paymentStatus.PaymentId);
            return new PaymentStatus(paymentId, paymentStatus.ClientId, paymentStatus.StoreId, !paymentId.ToString().Equals(ErrorPaymentId));
        }

        //use case 19 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/74
        public PaymentStatus CreatePayment(PaymentDetails paymentDetails)
        {
            Guid paymentId = _paymentSystem.CreatePayment(paymentDetails.ClientId, paymentDetails.ClientBankAccountId.AccountNumber, paymentDetails.ClientBankAccountId.Branch, paymentDetails.RecieverBankAccountId.AccountNumber, paymentDetails.RecieverBankAccountId.Branch, paymentDetails.PaymentSum);
            return new PaymentStatus(paymentId, paymentDetails.ClientId, paymentDetails.StoreId, !paymentId.ToString().Equals(ErrorPaymentId));
        }

    }
}
