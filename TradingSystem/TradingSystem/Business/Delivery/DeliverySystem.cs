using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public class DeliverySystem : ExternalDeliverySystem
    {
        private static readonly Lazy<DeliverySystem>
        _lazy =
        new Lazy<DeliverySystem>
            (() => new DeliverySystem());

        public static DeliverySystem Instance { get { return _lazy.Value; } }

        private DeliverySystem()
        {

        }

        public async Task<string> CreateDelivery(string username, string recieverPhone, double weight, string source, string destination)
        {
            return generatePackageId();
        }

        public async Task<string> CancelDelivery(string packageId)
        {
            return generatePackageId();
        }
        private string generatePackageId()
        {
            Random r = new Random();
            int rInt = r.Next(10000, 100000);
            return rInt.ToString();
        }
    }
}
