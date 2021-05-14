using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public interface ExternalDeliverySystem
    {
        public Task<string> CreateDelivery(string username, string recieverPhone, double weight, string source, string destination);
        public Task<string> CancelDelivery(string packageId);
    }
}
