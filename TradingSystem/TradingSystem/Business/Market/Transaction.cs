using Moq;
using System;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class Transaction
    {
        private PaymentAdapter _paymentAdapter;
        private DeliveryAdapter _deliveryAdapter;
        private static readonly Lazy<Transaction>
        _lazy =
        new Lazy<Transaction>
            (() => new Transaction());

        public static Transaction Instance { get { return _lazy.Value; } }

        public PaymentAdapter PaymentAdapter { get => _paymentAdapter; set => _paymentAdapter = value; }
        public DeliveryAdapter DeliveryAdapter { get => _deliveryAdapter; set => _deliveryAdapter = value; }

        private Transaction()
        {
            this.PaymentAdapter = new PaymentImpl();
            this.DeliveryAdapter = new DeliveryImpl();
        }

        internal void ActivateDebugMode(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem, bool debugMode)
        {
            if (debugMode)
            {
                this._deliveryAdapter.SetDeliverySystem(deliverySystem.Object);
                this._paymentAdapter.SetPaymentSystem(paymentSystem.Object);
            }
            else
            {
                this._deliveryAdapter = new DeliveryImpl();
                this._paymentAdapter = new PaymentImpl();
            }
        }

        public TransactionStatus ActivateTransaction(string username, string recieverPhone, double weight, Address source, Address destination, PaymentMethod method, Guid storeId, BankAccount recieverBankAccountId, double paymentSum, IShoppingBasket shoppingBasket)
        {
            TransactionStatus transactionStatus;
            DeliveryStatus deliveryStatus;
            PaymentStatus paymentStatus;
            var product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            DeliveryDetails deliveryDetails = new DeliveryDetails(username, storeId, recieverPhone, weight, source, destination);
            PaymentDetails paymentDetails = new PaymentDetails(username, method, storeId, recieverBankAccountId, paymentSum);
            ProductsStatus productsDetails = new ProductsStatus(username, storeId, product_quantity);
            paymentStatus = PaymentAdapter.CreatePayment(paymentDetails);
            //check if possible to deliver
            if (paymentStatus.Status)
            {
                deliveryStatus = DeliveryAdapter.CreateDelivery(deliveryDetails);
                transactionStatus = new TransactionStatus(paymentStatus, deliveryStatus, shoppingBasket, deliveryStatus.Status);
            }
            else
            {
                transactionStatus = new TransactionStatus(paymentStatus, null, shoppingBasket, false);
            }
            return transactionStatus;
        }

        public bool CancelTransaction(TransactionStatus transactionStatus, bool cancelPayments, bool cancelDeliveries)
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
            return allSucceeded;
        }

        public void DeleteAllTests()
        {
            this.PaymentAdapter = new PaymentImpl();
            this.DeliveryAdapter = new DeliveryImpl();

        }
    }
}
