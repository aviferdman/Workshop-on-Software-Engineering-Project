using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class DeliverySystem
    {
        private static readonly Lazy<DeliverySystem>
        lazy =
        new Lazy<DeliverySystem>
            (() => new DeliverySystem());

        public static DeliverySystem Instance { get { return lazy.Value; } }

        private DeliverySystem()
        {
        }
    }
}
