using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Delivery
{
    class DeliveryDetails
    {
        private string _recieverId;
        private string _recieverPhone;
        private double _weight;
        private string _source;
        private string _destination;

        public DeliveryDetails(string recieverId, string recieverPhone, double weight, string source, string destination)
        {
            this._recieverId = recieverId;
            this._recieverPhone = recieverPhone;
            this._weight = weight;
            this._source = source;
            this._destination = destination;
        }

        public string RecieverId { get => _recieverId; set => _recieverId = value; }
        public string RecieverPhone { get => _recieverPhone; set => _recieverPhone = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public string Source { get => _source; set => _source = value; }
        public string Destination { get => _destination; set => _destination = value; }
    }
}
