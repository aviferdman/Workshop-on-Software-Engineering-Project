using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;

namespace TradingSystem.Business.Payment
{
    public class RealDeliverySystem : ExternalDeliverySystem
    {
        private readonly ExternalSystemClient client;

        public RealDeliverySystem()
        {
            client = new ExternalSystemClient();
        }

        public async Task<string> CancelDelivery(string packageId)
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "cancel_supply" },
                { "transaction_id", packageId }
            };

            var responseString = await client.Send(postContent);
            return responseString;
        
        }

        public async Task<string> CreateDelivery(string username, string recieverPhone, double weight, string source, string destination)
        {
            throw new NotImplementedException();
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "supply" },
                { "name", username },
                { "address", "Rager Blvd 12" },
                { "city", "Beer Sheva" },
                { "country", "Israel" },
                { "zip", "8458527" }
            };

        }
    }
}
