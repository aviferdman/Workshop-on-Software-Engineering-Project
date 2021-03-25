using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class PaymentSystem
    {
        private static readonly Lazy<PaymentSystem>
        lazy =
        new Lazy<PaymentSystem>
            (() => new PaymentSystem());

        public static PaymentSystem Instance { get { return lazy.Value; } }

        private PaymentSystem()
        {
        }
    }
}
