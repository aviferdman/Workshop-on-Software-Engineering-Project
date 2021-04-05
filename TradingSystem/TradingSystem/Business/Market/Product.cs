using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Product
    {
        private Guid _id;
        private String _name;
        private int _quantity;
        private double _weight;
        private double _price;

        public Product(String name, int quantity, double weight, double price)
        {
            this._name = name;
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this._id = Guid.NewGuid();
        }

        public Product(int quantity, double weight, double price)
        {
            this._name = "test";
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this._id = Guid.NewGuid();
        }

        public Product(ProductData data)
        {
            this._name = data._name;
            this._quantity = data._quantity;
            this._weight = data._weight;
            this._price = data._price;
            this._id = Guid.NewGuid();
        }

        public String Name { get => _name; set => _name = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Guid Id { get => _id; set => _id = value; }
        public double Price { get => _price; set => _price = value; }
    }
}
