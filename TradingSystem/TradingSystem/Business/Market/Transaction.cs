using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class Transaction
    {
        private PaymentAdapter _paymentAdapter;
        private DeliveryAdapter _deliveryAdapter;
        private History _history;
        private static readonly Lazy<Transaction>
        _lazy =
        new Lazy<Transaction>
            (() => new Transaction());

        public static Transaction Instance { get { return _lazy.Value; } }

        private Transaction()
        {
            this._paymentAdapter = new PaymentImpl();
            this._deliveryAdapter = new DeliveryImpl();
            this._history = new History();
        }

        public TransactionStatus ActivateTransaction(Guid clientId, string recieverPhone, double weight, Address source, Address destination, BankAccount clientBankAccountId, Guid storeId, BankAccount recieverBankAccountId, double paymentSum)
        {
            TransactionStatus transactionStatus;
            DeliveryStatus deliveryStatus;
            PaymentStatus paymentStatus;
            DeliveryDetails deliveryDetails = new DeliveryDetails(clientId, storeId, recieverPhone, weight, source, destination);
            PaymentDetails paymentDetails = new PaymentDetails(clientId, clientBankAccountId, storeId, recieverBankAccountId, paymentSum);
            paymentStatus = _paymentAdapter.CreatePayment(paymentDetails);
            //check if possible to deliver
            if (paymentStatus.Status)
            {
                deliveryStatus = _deliveryAdapter.CreateDelivery(deliveryDetails);
                _history.AddDelivery(deliveryStatus);
                _history.AddPayment(paymentStatus);
                transactionStatus = new TransactionStatus(paymentStatus, deliveryStatus, deliveryStatus.Status);
            }
            else
            {
                transactionStatus = new TransactionStatus(paymentStatus, null, false);
            }
            return transactionStatus;
        }

        public History GetHistory(Guid userId)
        {
            return _history.GetHistory(userId);
        }

        internal History GetStoreHistory(Guid storeId)
        {
            return _history.GetStoreHistory(storeId);
        }

        public History GetAllHistory()
        {
            return _history;
        }

        public TransactionStatus CancelTransaction(TransactionStatus transactionStatus, bool cancelPayments, bool cancelDeliveries)
        {
            PaymentStatus paymentStatus = null;
            DeliveryStatus deliveryStatus = null;
            if (cancelPayments)
            {
                paymentStatus = _paymentAdapter.CancelPayment(transactionStatus.PaymentStatus);
            }
            if (cancelDeliveries)
            {
                deliveryStatus = _deliveryAdapter.CancelDelivery(transactionStatus.DeliveryStatus);
            }
            bool allSucceeded = deliveryStatus.Status && paymentStatus.Status;
            return new TransactionStatus(paymentStatus, deliveryStatus, allSucceeded);
        }
    }
}
