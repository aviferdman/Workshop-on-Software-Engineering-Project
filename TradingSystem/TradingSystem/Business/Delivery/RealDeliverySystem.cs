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

        public async Task<string> CreateDelivery(string name, string street, string city, string country, string zip)
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "supply" },
                { "name", name },
                { "address", street },
                { "city", city },
                { "country", country },
                { "zip", zip }
            };

            int numericValue;
            var responseString = await client.Send(postContent);
            bool isNumber = int.TryParse(responseString, out numericValue);
            if (!isNumber)
            {
                responseString = "-1";
            }
            else if (numericValue > 100000 || numericValue < 10000)
            {
                responseString = "-1";
            }

            return responseString;
        }
    }
}
