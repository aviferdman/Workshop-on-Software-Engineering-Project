using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class Transaction
    {
        private PaymentAdapter paymentAdapter;
        private DeliveryAdapter deliveryAdapter;
        private static readonly Lazy<Transaction>
        lazy =
        new Lazy<Transaction>
            (() => new Transaction());

        public static Transaction Instance { get { return lazy.Value; } }

        private Transaction()
        {
        }
    }
}
