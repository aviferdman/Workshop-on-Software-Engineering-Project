using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Payment
{
    public class PaymentSystem : ExternalPaymentSystem
    {
        private static readonly HttpClient client = new HttpClient();

        private static readonly Lazy<PaymentSystem>
        _lazy =
        new Lazy<PaymentSystem>
            (() => new PaymentSystem());

        public static PaymentSystem Instance { get { return _lazy.Value; } }

        private PaymentSystem()
        {
        }

        public async Task<string> CancelPayment(string paymentId)
        {
            return generatePackageId();
        }

        public virtual async Task<string> CreatePaymentAsync(string cardnumber, string month, string year, string holderName, string ccv, string id)
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
