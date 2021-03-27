using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    class Transaction
    {
        private PaymentAdapter _paymentAdapter;
        private DeliveryAdapter _deliveryAdapter;
        private static readonly Lazy<Transaction>
        _lazy =
        new Lazy<Transaction>
            (() => new Transaction());

        public static Transaction Instance { get { return _lazy.Value; } }

        private Transaction()
        {
            this._paymentAdapter = new PaymentImpl();
            this._deliveryAdapter = new DeliveryImpl();
        }
 
        public bool ActivateTransactionAsync(string clientId, string recieverPhone, double weight, string source, string destination, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            DeliveryStatus deliveryStatus;
            PaymentStatus paymentStatus;
            DeliveryDetails deliveryDetails = new DeliveryDetails(clientId, recieverPhone, weight, source, destination);
            PaymentDetails paymentDetails = new PaymentDetails(clientId, clientBankAccountId, recieverBankAccountId, paymentSum);
            deliveryStatus = _deliveryAdapter.CreateDelivery(deliveryDetails);
            //check if possible to deliver
            if (deliveryStatus.Status)
            {
                paymentStatus = _paymentAdapter.CreatePayment(paymentDetails);
                //check if possible to pay
                if (!paymentStatus.Status)
                {
                    _deliveryAdapter.CancelDelivery(deliveryDetails);
                    return false;
                }
                return true;
            }
            else
            {
                _deliveryAdapter.CancelDelivery(deliveryDetails);
                return false;
            }

        }
    }
}
