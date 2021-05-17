using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    public interface ExternalPaymentSystem
    {
        public Task<string> CancelPayment(string paymentId);

        public Task<string> CreatePaymentAsync(string cardnumber, string month, string year, string holderName, string ccv, string id);
    }
}
