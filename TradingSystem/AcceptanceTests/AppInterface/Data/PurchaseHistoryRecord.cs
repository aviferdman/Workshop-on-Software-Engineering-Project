using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class PurchaseHistoryRecord : IEnumerable<ProductInCart>
    {
        public PurchaseHistoryRecord
        (
            IEnumerable<ProductInCart> products,
            Guid deliveryPackageId,
            bool deliveryStatus,
            Guid paymentId,
            bool paymentStatus
        )
        {
            Products = products;
            DeliveryPackageId = deliveryPackageId;
            DeliveryStatus = deliveryStatus;
            PaymentId = paymentId;
            PaymentStatus = paymentStatus;
        }

        public IEnumerable<ProductInCart> Products { get; }

        public Guid DeliveryPackageId { get; }
        public bool DeliveryStatus { get; }

        public Guid PaymentId { get; }
        public bool PaymentStatus { get; }

        public IEnumerator<ProductInCart> GetEnumerator()
        {
            return Products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Products).GetEnumerator();
        }
    }
}
