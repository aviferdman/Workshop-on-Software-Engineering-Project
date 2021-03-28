using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class DeliveryStatus
    {
        Guid _packageId;
        Guid _clientId;
        Guid _storeId;
        bool _status;

        public DeliveryStatus(Guid packageId, Guid clientId, Guid storeId, bool status)
        {
            this._packageId = packageId;
            this._clientId = clientId;
            this.StoreId = storeId;
            this._status = status;
        }

        public Guid PackageId { get => _packageId; set => _packageId = value; }
        public bool Status { get => _status; set => _status = value; }
        public Guid ClientId { get => _clientId; set => _clientId = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
    }
}
