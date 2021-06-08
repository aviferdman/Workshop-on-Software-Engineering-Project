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

        public async Task<string> CreatePaymentAsync(string cardnumber, string month, string year, string holderName, string ccv, string id)
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "pay" },
                { "card_number", cardnumber },
                { "month", month },
                { "year", year },
                { "holder", holderName },
                { "ccv", ccv },
                { "id", id }
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
