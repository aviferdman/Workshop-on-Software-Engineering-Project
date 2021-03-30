using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Product
    {
        private Guid _id;
        private int _quantity;
        private double _weight;
        private double _price;

        public Product(int quantity, double weight, double price)
        {
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this._id = new Guid();
        }

        public int Quantity { get => _quantity; set => _quantity = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Guid Id { get => _id; set => _id = value; }
        public double Price { get => _price; set => _price = value; }
    }
}
