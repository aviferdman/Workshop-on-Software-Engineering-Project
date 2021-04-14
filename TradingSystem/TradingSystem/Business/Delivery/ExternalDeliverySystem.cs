using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    public interface ExternalDeliverySystem
    {
        public Guid CreateDelivery(Guid recieverId, string recieverPhone, double weight, string source, string destination);
        public Guid CancelDelivery(Guid packageId);
    }
}
