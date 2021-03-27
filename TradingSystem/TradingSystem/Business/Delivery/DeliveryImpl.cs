using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    class DeliveryImpl : DeliveryAdapter
    {
        private readonly string ErrorPackageId = "";
        private DeliverySystem _deliverySystem;

        public DeliveryImpl()
        {
            this._deliverySystem = DeliverySystem.Instance;
        }

        public DeliveryStatus CancelDelivery(DeliveryDetails deliveryDetails)
        {
            string packageId = _deliverySystem.CancelDelivery(deliveryDetails.RecieverId, deliveryDetails.RecieverPhone, deliveryDetails.Weight, deliveryDetails.Source, deliveryDetails.Destination);
            return new DeliveryStatus(packageId, !packageId.Equals(ErrorPackageId));
        
        }

        //use case 42 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/73
        public DeliveryStatus CreateDelivery(DeliveryDetails deliveryDetails)
        {
            string packageId =_deliverySystem.CreateDelivery(deliveryDetails.RecieverId, deliveryDetails.RecieverPhone, deliveryDetails.Weight, deliveryDetails.Source, deliveryDetails.Destination);
            return new DeliveryStatus(packageId, !packageId.Equals(ErrorPackageId));
        }
    }
}
