using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryImpl : DeliveryAdapter
    {
        private readonly Guid ErrorPackageId = new Guid();
        private ExternalDeliverySystem _deliverySystem;

        public DeliveryImpl(ExternalDeliverySystem externalDeliverySystem = null)
        {
            //proxy
            if (externalDeliverySystem != null)
            {
                this._deliverySystem = externalDeliverySystem;
            }
            else
            {
                this._deliverySystem = DeliverySystem.Instance;
            }            
        }

        public DeliveryStatus CancelDelivery(DeliveryStatus deliveryStatus)
        {
            //Guid packageId = _deliverySystem.CancelDelivery(deliveryStatus.PackageId);
            //return new DeliveryStatus(packageId, deliveryStatus.Username, deliveryStatus.StoreId, !packageId.Equals(ErrorPackageId));
            return null;
        }

        //use case 42 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/73
        public DeliveryStatus CreateDelivery(DeliveryDetails deliveryDetails)
        {
            Guid packageId =_deliverySystem.CreateDelivery(deliveryDetails.Username, deliveryDetails.RecieverPhone, deliveryDetails.Weight, deliveryDetails.Source.ToString(), deliveryDetails.Destination.ToString());
            return new DeliveryStatus(packageId, deliveryDetails.Username, deliveryDetails.StoreId, !packageId.Equals(ErrorPackageId));
        }

        public void SetDeliverySystem(ExternalDeliverySystem externalDeliverySystem)
        {
            this._deliverySystem = externalDeliverySystem;
        }
    }
}
