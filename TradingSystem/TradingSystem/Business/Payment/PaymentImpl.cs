using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Delivery
{
    public class PaymentImpl : PaymentAdapter
    {
        private readonly string ErrorPaymentId = "-1";
        private ExternalPaymentSystem _paymentSystem;        
        
        public PaymentImpl(ExternalPaymentSystem paymentSystem = null)
        {
            //proxy
            if (paymentSystem != null)
            {
                this._paymentSystem = paymentSystem;
            }
            else
            {
                this._paymentSystem = PaymentSystem.Instance;
            }
        }

        public async Task<PaymentStatus> CancelPayment(PaymentStatus paymentStatus)
        {
            string paymentId = await _paymentSystem.CancelPayment(paymentStatus.PaymentId);
            return new PaymentStatus(paymentId, paymentStatus.Username, paymentStatus.StoreId, !paymentId.Equals(ErrorPaymentId));
        }

        //use case 19 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/74
        public async Task<PaymentStatus> CreatePayment(PaymentDetails paymentDetails)
        {
            string paymentId = await _paymentSystem.CreatePaymentAsync(paymentDetails.Method.GetCardNumber(), paymentDetails.Method.GetMonth(), paymentDetails.Method.GetYear(), paymentDetails.Method.GetHolderName(), paymentDetails.Method.GetCVV(), paymentDetails.Method.GetHolderId());
            return new PaymentStatus(paymentId, paymentDetails.Username, paymentDetails.StoreId, !paymentId.ToString().Equals(ErrorPaymentId));
        }

        public bool SetPaymentSystem(ExternalPaymentSystem externalPaymentSystem)
        {
            this._paymentSystem = externalPaymentSystem;
            return true;
        }
    }
}
