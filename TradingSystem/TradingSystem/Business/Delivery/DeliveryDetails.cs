using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryDetails
    {
        private string _username;
        private Guid _storeId;
        private string _storeName;
        private string _recieverPhone;
        private double _weight;
        private Address _source;
        private Address _destination;

        public DeliveryDetails(string username, Guid storeId, string storeName, string recieverPhone, double weight, Address source, Address destination)
        {
            this.Username = username;
            this.StoreId = storeId;
            this.StoreName = storeName;
            this._recieverPhone = recieverPhone;
            this._weight = weight;
            this._source = source;
            this._destination = destination;
        }

        public string RecieverPhone { get => _recieverPhone; set => _recieverPhone = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Address Source { get => _source; set => _source = value; }
        public Address Destination { get => _destination; set => _destination = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public string Username { get => _username; set => _username = value; }
        public string StoreName { get => _storeName; set => _storeName = value; }
    }
}
