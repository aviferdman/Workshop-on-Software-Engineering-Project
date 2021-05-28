using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryStatus
    {
        string _packageId;
        string _username;
        Guid _storeId;
        string storeName;
        bool _status;

        public DeliveryStatus(string packageId, string username, Guid storeId, string storeName, bool status)
        {
            this._packageId = packageId;
            this.Username = username;
            this.StoreId = storeId;
            this.StoreName = storeName;
            this._status = status;
        }

        public string PackageId { get => _packageId; set => _packageId = value; }
        public bool Status { get => _status; set => _status = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public string Username { get => _username; set => _username = value; }
        public string StoreName { get => storeName; set => storeName = value; }
    }
}
