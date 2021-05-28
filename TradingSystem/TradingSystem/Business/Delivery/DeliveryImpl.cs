using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryImpl : DeliveryAdapter
    {
        private readonly string ErrorPackageId = "-1";
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

        public async Task<DeliveryStatus> CancelDelivery(DeliveryStatus deliveryStatus)
        {
            string packageId = await _deliverySystem.CancelDelivery(deliveryStatus.PackageId);
            return new DeliveryStatus(packageId, deliveryStatus.Username, deliveryStatus.StoreId, deliveryStatus.StoreName, !packageId.ToString().Equals(ErrorPackageId));
        }

        //use case 42 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/73
        public async Task<DeliveryStatus> CreateDelivery(DeliveryDetails deliveryDetails)
        {
            string packageId = await _deliverySystem.CreateDelivery(deliveryDetails.Username, $"{deliveryDetails.Destination.Street} {deliveryDetails.Destination.ApartmentNum}", deliveryDetails.Destination.City, deliveryDetails.Destination.State, deliveryDetails.Destination.Zip);
            return new DeliveryStatus(packageId, deliveryDetails.Username, deliveryDetails.StoreId, deliveryDetails.StoreName, !packageId.ToString().Equals(ErrorPackageId));
        }

        public bool SetDeliverySystem(ExternalDeliverySystem externalDeliverySystem)
        {
            this._deliverySystem = externalDeliverySystem;
            return true;
        }
    }
}
