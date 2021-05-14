using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Payment
{
    public interface ExternalPaymentSystem
    {
        public Task<string> CancelPayment(string paymentId);

        public Task<string> CreatePaymentAsync(string username, string paymentMethod, int accountNumber2, int branch2, double paymentSum);
    }
}
