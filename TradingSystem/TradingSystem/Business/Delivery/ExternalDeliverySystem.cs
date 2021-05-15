using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public interface ExternalDeliverySystem
    {
        public Task<string> CreateDelivery(string name, string street, string city, string country, string zip);
        public Task<string> CancelDelivery(string packageId);
    }
}
