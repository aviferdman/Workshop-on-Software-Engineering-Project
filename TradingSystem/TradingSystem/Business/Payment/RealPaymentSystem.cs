using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    public class RealPaymentSystem : ExternalPaymentSystem
    {
        private readonly ExternalSystemClient client;

        public RealPaymentSystem()
        {
            client = new ExternalSystemClient();
        }
        public async Task<string> CancelPayment(string paymentId)
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "cancel_pay" },
                { "transaction_id", paymentId }
            };

            var responseString = await client.Send(postContent);
            return responseString;
        }

        public async Task<string> CreatePaymentAsync(string username, string paymentMethod, int accountNumber2, int branch2, double paymentSum)
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "pay" },
                { "card_number", "2222333344445555" },
                { "month", "4" },
                { "year", "2021" },
                { "holder", "Israel Israelovice" },
                { "ccv", "262" },
                { "id", "20444444" }
            };

            var responseString = await client.Send(postContent);
            return responseString;
        }
    }
}
