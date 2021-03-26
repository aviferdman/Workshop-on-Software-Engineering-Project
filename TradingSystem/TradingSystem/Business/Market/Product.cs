using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Product
    {
        private int _quantity;
        private double weight;
        private string id;

        public Product()
        {
            this.Quantity = 0;
        }

        public int Quantity { get => _quantity; set => _quantity = value; }
        public double Weight { get => weight; set => weight = value; }
        public string Id { get => id; set => id = value; }
    }
}
