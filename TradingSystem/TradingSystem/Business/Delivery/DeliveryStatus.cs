using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class DeliveryStatus
    {
        Guid _packageId;
        bool _status;

        public DeliveryStatus(Guid packageId, bool status)
        {
            this._packageId = packageId;
            this._status = status;
        }

        public Guid PackageId { get => _packageId; set => _packageId = value; }
        public bool Status { get => _status; set => _status = value; }
    }
}
