using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public interface DeliveryAdapter
    {
        public bool SetDeliverySystem(ExternalDeliverySystem externalDeliverySystem);
        public Task<DeliveryStatus> CreateDelivery(DeliveryDetails deliveryDetails);
        public Task<DeliveryStatus> CancelDelivery(DeliveryStatus deliveryStatus);
    }
}
