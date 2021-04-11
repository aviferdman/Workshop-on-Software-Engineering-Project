using Moq;
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

        public PaymentAdapter PaymentAdapter { get => _paymentAdapter; set => _paymentAdapter = value; }
        public DeliveryAdapter DeliveryAdapter { get => _deliveryAdapter; set => _deliveryAdapter = value; }
        public History History { get => _history; set => _history = value; }

        private Transaction()
        {
            this.PaymentAdapter = new PaymentImpl();
            this.DeliveryAdapter = new DeliveryImpl();
            this.History = new History();
        }

        internal void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode)
        {
            if (debugMode)
            {
                this._deliveryAdapter = deliveryAdapter.Object;
                this._paymentAdapter = paymentAdapter.Object;
            }
            else
            {
                this._deliveryAdapter = new DeliveryImpl();
                this._paymentAdapter = new PaymentImpl();
            }
        }

        public TransactionStatus ActivateTransaction(Guid clientId, string recieverPhone, double weight, Address source, Address destination, BankAccount clientBankAccountId, Guid storeId, BankAccount recieverBankAccountId, double paymentSum, Dictionary<Product, int> product_quantity)
        {
            TransactionStatus transactionStatus;
            DeliveryStatus deliveryStatus;
            PaymentStatus paymentStatus;
            DeliveryDetails deliveryDetails = new DeliveryDetails(clientId, storeId, recieverPhone, weight, source, destination);
            PaymentDetails paymentDetails = new PaymentDetails(clientId, clientBankAccountId, storeId, recieverBankAccountId, paymentSum);
            ProductsStatus productsDetails = new ProductsStatus(clientId, storeId, product_quantity);
            paymentStatus = PaymentAdapter.CreatePayment(paymentDetails);
            //check if possible to deliver
            if (paymentStatus.Status)
            {
                deliveryStatus = DeliveryAdapter.CreateDelivery(deliveryDetails);
                History.AddDelivery(deliveryStatus);
                History.AddPayment(paymentStatus);
                History.AddProductsQuantity(productsDetails);
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
            return History.GetHistory(userId);
        }

        internal History GetStoreHistory(Guid storeId)
        {
            return History.GetStoreHistory(storeId);
        }

        public History GetAllHistory()
        {
            return History;
        }

        public TransactionStatus CancelTransaction(TransactionStatus transactionStatus, bool cancelPayments, bool cancelDeliveries)
        {
            PaymentStatus paymentStatus = null;
            DeliveryStatus deliveryStatus = null;
            if (cancelPayments)
            {
                paymentStatus = PaymentAdapter.CancelPayment(transactionStatus.PaymentStatus);
            }
            if (cancelDeliveries)
            {
                deliveryStatus = DeliveryAdapter.CancelDelivery(transactionStatus.DeliveryStatus);
            }
            bool allSucceeded = (deliveryStatus == null || deliveryStatus.Status) && (paymentStatus == null || paymentStatus.Status);
            return new TransactionStatus(paymentStatus, deliveryStatus, allSucceeded);
        }

        public void DeleteAllTests()
        {
            this.PaymentAdapter = new PaymentImpl();
            this.DeliveryAdapter = new DeliveryImpl();
            this.History = new History();
        }
    }
}
