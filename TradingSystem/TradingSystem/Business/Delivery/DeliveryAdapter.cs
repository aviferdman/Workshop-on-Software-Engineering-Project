﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Delivery
{
    public interface DeliveryAdapter
    {
        public void SetDeliverySystem(ExternalDeliverySystem externalDeliverySystem);
        public DeliveryStatus CreateDelivery(DeliveryDetails deliveryDetails);
        public DeliveryStatus CancelDelivery(DeliveryStatus deliveryStatus);
    }
}
