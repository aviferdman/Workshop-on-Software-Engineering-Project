using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryImpl : DeliveryAdapter
    {
        private readonly string ErrorPackageId = "";
        private DeliverySystem _deliverySystem;

        public DeliveryImpl()
        {
            this._deliverySystem = DeliverySystem.Instance;
        }

        public DeliveryStatus CancelDelivery(DeliveryStatus deliveryStatus)
        {
            Guid packageId = _deliverySystem.CancelDelivery(deliveryStatus.PackageId);
            return new DeliveryStatus(packageId, deliveryStatus.ClientId, deliveryStatus.StoreId, !packageId.ToString().Equals(ErrorPackageId));
        }

        //use case 42 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/73
        public DeliveryStatus CreateDelivery(DeliveryDetails deliveryDetails)
        {
            Guid packageId =_deliverySystem.CreateDelivery(deliveryDetails.RecieverId, deliveryDetails.RecieverPhone, deliveryDetails.Weight, deliveryDetails.Source.ToString(), deliveryDetails.Destination.ToString());
            return new DeliveryStatus(packageId, deliveryDetails.RecieverId, deliveryDetails.StoreId, !packageId.ToString().Equals(ErrorPackageId));
        }
    }
}
