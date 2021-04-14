using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public interface PaymentMethod
    {
        public String GeneratePaymentDetails();
    }
}
