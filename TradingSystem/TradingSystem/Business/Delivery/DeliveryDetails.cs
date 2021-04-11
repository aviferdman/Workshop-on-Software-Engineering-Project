using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Delivery
{
    public class DeliveryDetails
    {
        private Guid _recieverId;
        private Guid _storeId;
        private string _recieverPhone;
        private double _weight;
        private Address _source;
        private Address _destination;

        public DeliveryDetails(Guid recieverId, Guid storeId, string recieverPhone, double weight, Address source, Address destination)
        {
            this._recieverId = recieverId;
            this.StoreId = storeId;
            this._recieverPhone = recieverPhone;
            this._weight = weight;
            this._source = source;
            this._destination = destination;
        }

        public Guid RecieverId { get => _recieverId; set => _recieverId = value; }
        public string RecieverPhone { get => _recieverPhone; set => _recieverPhone = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Address Source { get => _source; set => _source = value; }
        public Address Destination { get => _destination; set => _destination = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
    }
}
