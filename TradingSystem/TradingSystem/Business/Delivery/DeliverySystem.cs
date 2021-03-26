using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    class DeliverySystem
    {
        private static readonly Lazy<DeliverySystem>
        _lazy =
        new Lazy<DeliverySystem>
            (() => new DeliverySystem());

        public static DeliverySystem Instance { get { return _lazy.Value; } }

        private DeliverySystem()
        {
        }

        internal Task<bool> CreateDelivery(string recieverId, string recieverPhone, double weight, string source, string destination)
        {
            return new Task<bool>(() => true);
        }

        internal Task<bool> CancelDelivery(object recieverId, string recieverPhone, double weight, string source, string destination)
        {
            return new Task<bool>(() => true);
        }
    }
}
