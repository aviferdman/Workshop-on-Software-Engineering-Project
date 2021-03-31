using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class History
    {
        ICollection<DeliveryStatus> _deliveries;
        ICollection<PaymentStatus> _payments;
        ICollection<ProductsStatus> _products;

        public History()
        {
            this.Deliveries = new HashSet<DeliveryStatus>();
            this.Payments = new HashSet<PaymentStatus>();
            this.Products = new HashSet<ProductsStatus>();
        }

        private History(IEnumerable<DeliveryStatus> deliveries, IEnumerable<PaymentStatus> payments, IEnumerable<ProductsStatus> products)
        {
            this.Deliveries = deliveries.ToList();
            this.Payments = payments.ToList();
            this.Products = products.ToList();
        }

        public ICollection<DeliveryStatus> Deliveries { get => _deliveries; set => _deliveries = value; }
        public ICollection<PaymentStatus> Payments { get => _payments; set => _payments = value; }
        public ICollection<ProductsStatus> Products { get => _products; set => _products = value; }

        public void AddDelivery(DeliveryStatus deliveryStatus)
        {
            Deliveries.Add(deliveryStatus);
        }

        public void AddPayment(PaymentStatus paymentStatus)
        {
            Payments.Add(paymentStatus);
        }

        public void AddProductsQuantity(ProductsStatus productsStatus)
        {
            Products.Add(productsStatus);
        }

        public History GetHistory(Guid userId)
        {
            var deliveries = Deliveries.Where( d => d.ClientId.Equals(userId));
            var payments = Payments.Where( p => p.ClientId.Equals(userId));
            var products = Products.Where( p => p.ClientId.Equals(userId));
            return new History(deliveries, payments, products);
        }

        internal History GetStoreHistory(Guid storeId)
        {
            var deliveries = Deliveries.Where(d => d.StoreId.Equals(storeId));
            var payments = Payments.Where(p => p.StoreId.Equals(storeId));
            var products = Products.Where(p => p.StoreId.Equals(storeId));
            return new History(deliveries, payments, products);
        }
    }
}
