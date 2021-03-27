using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    interface DeliveryAdapter
    {
        public DeliveryStatus CreateDelivery(DeliveryDetails deliveryDetails);
        public DeliveryStatus CancelDelivery(DeliveryDetails deliveryDetails);
    }
}
