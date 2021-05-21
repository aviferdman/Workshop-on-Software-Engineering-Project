using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Product
    {
        public Guid _id { get; set; }
        public String _name { get; set; }
        public int _quantity { get; set; }
        public double _weight { get; set; }
        public double _price { get; set; }
        public String category { get; set; }
        public int rating { get; set; }

        public Product(String name, int quantity, double weight, double price, String category)
        {
            this._name = name;
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this._id = Guid.NewGuid();
            this.category = category;
            rating = -1;
        }

        public Product(int quantity, double weight, double price)
        {
            this._name = "test";
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this._id = Guid.NewGuid();
            category = null;
            rating = -1;
        }

        public Product(ProductData data)
        {
            this._name = data._name;
            this._quantity = data._quantity;
            this._weight = data._weight;
            this._price = data._price;
            this.category = data.category;
            this.rating = data.rating;
            this._id = Guid.NewGuid();
        }

        public String Name { get => _name; set => _name = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public Guid Id { get => _id; set => _id = value; }
        public double Price { get => _price; set => _price = value; }
        public string Category { get => category; set => category = value; }
        public int Rating { get => rating; set => rating = value; }
    }
}
