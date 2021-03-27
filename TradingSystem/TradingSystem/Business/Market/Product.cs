using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Product
    {
        private int _quantity;
        private double _weight;
        private Guid _id;

        public Product()
        {
            this.Quantity = 0;
        }

        public int Quantity { get => _quantity; set => _quantity = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Guid Id { get => _id; set => _id = value; }
    }
}
