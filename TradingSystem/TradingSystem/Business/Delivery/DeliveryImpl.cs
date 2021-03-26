using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    class DeliveryImpl : DeliveryAdapter
    {
        private DeliverySystem _deliverySystem;

        public DeliveryImpl()
        {
            this._deliverySystem = DeliverySystem.Instance;
        }

        public Task<bool> CancelDelivery(string recieverId, string recieverPhone, double weight, string source, string destination)
        {
            return _deliverySystem.CancelDelivery(recieverId, recieverPhone, weight, source, destination);
        }

        //use case 42 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/73
        public Task<bool> CreateDelivery(string recieverId, string recieverPhone, double weight, string source, string destination)
        {
            return _deliverySystem.CreateDelivery(recieverId, recieverPhone, weight, source, destination);
        }
    }
}
