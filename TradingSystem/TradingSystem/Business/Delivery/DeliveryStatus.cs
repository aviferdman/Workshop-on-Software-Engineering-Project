using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryStatus
    {
        Guid _packageId;
        string _username;
        Guid _storeId;
        bool _status;

        public DeliveryStatus(Guid packageId, string username, Guid storeId, bool status)
        {
            this._packageId = packageId;
            this.Username = username;
            this.StoreId = storeId;
            this._status = status;
        }

        public Guid PackageId { get => _packageId; set => _packageId = value; }
        public bool Status { get => _status; set => _status = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public string Username { get => _username; set => _username = value; }
    }
}
