using Moq;
using System;
using System.Configuration;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Handshake;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class Transaction
    {
        private PaymentAdapter _paymentAdapter;
        private DeliveryAdapter _deliveryAdapter;
        private HandshakeAdapter _handshakeAdapter;
        private static readonly Lazy<Transaction>
        _lazy =
        new Lazy<Transaction>
            (() => new Transaction());

        public static Transaction Instance { get { return _lazy.Value; } }

        public PaymentAdapter PaymentAdapter { get => _paymentAdapter; set => _paymentAdapter = value; }
        public DeliveryAdapter DeliveryAdapter { get => _deliveryAdapter; set => _deliveryAdapter = value; }
        public HandshakeAdapter HandshakeAdapter { get => _handshakeAdapter; set => _handshakeAdapter = value; }

        private Transaction()
        {
            string enabled = ConfigurationManager.AppSettings["EnableRealExternalSystems"]; 
            bool enableExternal = enabled != null? enabled.ToLower().Equals("true") : false;
            if (enableExternal)
            {
                this.PaymentAdapter = new PaymentImpl(new RealPaymentSystem());
                this.DeliveryAdapter = new DeliveryImpl(new RealDeliverySystem());
                this.HandshakeAdapter = new HandshakeImpl(new RealHandshakeSystem());
            }
            else
            {
                this.PaymentAdapter = new PaymentImpl();
                this.DeliveryAdapter = new DeliveryImpl();
                this.HandshakeAdapter = new HandshakeImpl();
            }
        }

        internal bool ActivateDebugMode(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem, bool debugMode)
        {
            bool connectExternalSystems = true;
            if (debugMode)
            {
                connectExternalSystems = connectExternalSystems && this._deliveryAdapter.SetDeliverySystem(deliverySystem.Object);
                connectExternalSystems = connectExternalSystems && this._paymentAdapter.SetPaymentSystem(paymentSystem.Object);
            }
            else
            {
                this._deliveryAdapter = new DeliveryImpl();
                this._paymentAdapter = new PaymentImpl();
                connectExternalSystems = connectExternalSystems && this._deliveryAdapter.SetDeliverySystem(DeliverySystem.Instance);
                connectExternalSystems = connectExternalSystems && this._paymentAdapter.SetPaymentSystem(PaymentSystem.Instance);
            }
            if (!connectExternalSystems)
            {
                Logger.Instance.MonitorError("Error: " + nameof(Transaction) + " " + nameof(ActivateDebugMode));
            }
            return connectExternalSystems;
        }
        //TODO fixs
        public async Task<TransactionStatus> ActivateTransaction(string username, string recieverPhone, double weight, Address source, Address destination, PaymentMethod method, Guid storeId, CreditCard recieverBankAccountId, double paymentSum, ShoppingBasket shoppingBasket)
        {
            var product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            DeliveryDetails deliveryDetails = new DeliveryDetails(username, storeId, recieverPhone, weight, source, destination);
            PaymentDetails paymentDetails = new PaymentDetails(username, method, storeId, recieverBankAccountId, paymentSum);
            ProductsStatus productsDetails = new ProductsStatus(username, storeId, product_quantity);
            bool handshake = await _handshakeAdapter.CheckAvailablity();
            //handshake
            if (!handshake)
            {
                return new TransactionStatus(username, storeId, null, null, shoppingBasket, false);
            }
            //payment
            var paymentStatus = await PaymentAdapter.CreatePayment(paymentDetails);
            if (!paymentStatus.Status)
            {
                return new TransactionStatus(username, storeId, paymentStatus, null, shoppingBasket, false);
            }
            //deliver
            var deliveryStatus = await DeliveryAdapter.CreateDelivery(deliveryDetails);

            return new TransactionStatus(username, storeId, paymentStatus, deliveryStatus, shoppingBasket, deliveryStatus.Status);
        }

        public async Task<bool> CancelTransaction(TransactionStatus transactionStatus, bool cancelPayments, bool cancelDeliveries)
        {
            Logger.Instance.MonitorActivity(nameof(Transaction) + " " + nameof(CancelTransaction));
            PaymentStatus paymentStatus = null;
            DeliveryStatus deliveryStatus = null;
            bool handshake = await _handshakeAdapter.CheckAvailablity();
            // handshake
            if (!handshake)
            {
                return false;
            }
            if (cancelPayments)
            {
                paymentStatus = await PaymentAdapter.CancelPayment(transactionStatus.PaymentStatus);
            }
            if (cancelDeliveries)
            {
                deliveryStatus = await DeliveryAdapter.CancelDelivery(transactionStatus.DeliveryStatus);
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
