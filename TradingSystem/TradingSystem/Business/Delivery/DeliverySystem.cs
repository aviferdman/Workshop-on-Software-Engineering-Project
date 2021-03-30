using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public class DeliverySystem
    {
        private static readonly Lazy<DeliverySystem>
        _lazy =
        new Lazy<DeliverySystem>
            (() => new DeliverySystem());

        public static DeliverySystem Instance { get { return _lazy.Value; } }

        private DeliverySystem()
        {
        }

        public Guid CreateDelivery(Guid recieverId, string recieverPhone, double weight, string source, string destination)
        {
            return generatePackageId();
        }

        public Guid CancelDelivery(Guid packageId)
        {
            return generatePackageId();
        }
        private Guid generatePackageId()
        {
            return new Guid();
        }
    }
}
