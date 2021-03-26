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
 
        public async Task<bool> ActivateTransaction(string clientId, string recieverPhone, double weight, string source, string destination, string clientBankAccountId, string recieverBankAccountId, double paymentSum)
        {
            bool deliveryStatus, paymentStatus;
            deliveryStatus = await _deliveryAdapter.CreateDelivery(clientId, recieverPhone, weight, source, destination);
            if (deliveryStatus)
            {
                paymentStatus = await _paymentAdapter.CreatePayment(clientId, clientBankAccountId, recieverBankAccountId, paymentSum);
                if (!paymentStatus)
                {
                    await _deliveryAdapter.CancelDelivery(clientId, recieverPhone, weight, source, destination);
                    return false;
                }
                return true;
            }
            else
            {
                await _deliveryAdapter.CancelDelivery(clientId, recieverPhone, weight, source, destination);
                return false;
            }

        }
    }
}
