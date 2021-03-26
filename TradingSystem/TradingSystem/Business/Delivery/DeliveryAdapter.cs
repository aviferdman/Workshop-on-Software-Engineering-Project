using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    interface DeliveryAdapter
    {
        public Task<bool> CreateDelivery(string recieverId, string recieverPhone, double weight, string source, string destination);
        public Task<bool> CancelDelivery(string clientId, string recieverPhone, double weight, string source, string destination);
    }
}
