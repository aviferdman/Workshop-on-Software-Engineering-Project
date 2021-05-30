using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class TransactionStatus
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public Guid storeID { get; set; }
        public PaymentStatus _paymentStatus { get; set; }
        public DeliveryStatus _deliveryStatus { get; set; }
        public ProductHistoryData productHistories { get; set; }
        public bool _status { get; set; }

        public TransactionStatus(string username, Guid storeId, PaymentStatus paymentStatus, DeliveryStatus deliveryStatus, ShoppingBasket shoppingBasket, bool status)
        {
            this._paymentStatus = paymentStatus;
            this._deliveryStatus = deliveryStatus;
            this.Status = status;
            this.id = Guid.NewGuid();
            this.username = username;
            this.storeID = storeId;
            this.ProductHistories = new ProductHistoryData();
            var tmpDict = shoppingBasket?.GetDictionaryProductQuantity() ?? new HashSet<UserPackage.ProductInCart>();
            foreach (var p_q in tmpDict)
            {
                ProductHistories.Add(p_q.product, p_q.quantity);
            }
        }

        public TransactionStatus()
        {
        }

        /*
        public TransactionStatus(  PaymentStatus paymentStatus, DeliveryStatus deliveryStatus, ShoppingBasket shoppingBasket, bool status)
        {
            this._paymentStatus = paymentStatus;
            this._deliveryStatus = deliveryStatus;
            this.Status = status;
            this._id = Guid.NewGuid();
            this.ProductHistories = new ProductHistoryData();
            var tmpDict = shoppingBasket?.GetDictionaryProductQuantity() ?? new HashSet<UserPackage.ProductInCart>();
            foreach (var p_q in tmpDict)
            {
                ProductHistories.Add(p_q.product, p_q.quantity);
            }
        }
        */
        [NotMapped]
        public Guid Id { get => id; set => id = value; }
        [NotMapped]
        public bool Status { get => _status; set => _status = value; }
        [NotMapped]
        public DeliveryStatus DeliveryStatus { get => _deliveryStatus; set => _deliveryStatus = value; }
        [NotMapped]
        public PaymentStatus PaymentStatus { get => _paymentStatus; set => _paymentStatus = value; }
        [NotMapped]
        public ProductHistoryData ProductHistories { get => productHistories; set => productHistories = value; }
    }
}
