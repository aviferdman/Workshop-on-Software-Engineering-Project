using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class DeliveryStatus
    {
        string _packageId;
        bool _status;

        public DeliveryStatus(string packageId, bool status)
        {
            this._packageId = packageId;
            this._status = status;
        }

        public string PackageId { get => _packageId; set => _packageId = value; }
        public bool Status { get => _status; set => _status = value; }
    }
}
