using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Handshake
{
    class RealHandshakeSystem : ExternalHandshakeSystem
    {
        private readonly ExternalSystemClient client;

        public RealHandshakeSystem()
        {
            client = new ExternalSystemClient();
        }
        public async Task<bool> CheckAvailablity()
        {
            var postContent = new Dictionary<string, string>
            {
             { "action_type", "handshake" },
            };

            var responseString = await client.Send(postContent);

            return responseString.ToLower().Equals("ok");
        }
    }
}
