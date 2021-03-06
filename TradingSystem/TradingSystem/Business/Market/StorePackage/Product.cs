using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Product
    {
        public Guid id { get; set; }

        public Guid _storeId { get; set; }
        public String _name { get; set; }
        public int _quantity { get; set; }
        public double _weight { get; set; }
        public double _price { get; set; }
        public String category { get; set; }
        public int rating { get; set; }

        public string _storeName { get; set; }

        public double discount { get; set; }

        public Product(Guid storeId, String name, int quantity, double weight, double price, String category, string storeName = "")
        {
            _storeId = storeId;
            this._name = name;
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this.id = Guid.NewGuid();
            this.category = category;
            rating = -1;
            this.StoreName = storeName;
        }

        public Product(Guid storeId, int quantity, double weight, double price)
        {
            this._storeId = storeId;
            this._name = "test";
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this.id = Guid.NewGuid();
            category = null;
            rating = -1;
        }

        public Product(ProductData data)
        {
            this._storeId = data._storeId;
            this._name = data._name;
            this._quantity = data._quantity;
            this._weight = data._weight;
            this._price = data._price;
            this.category = data.category;
            this.rating = data.rating;
            this.id = Guid.NewGuid();
            this.StoreName = data.StoreName;
        }
        [NotMapped]
        public String Name { get => _name; set => _name = value; }
        [NotMapped]
        public int Quantity { get => _quantity; set => _quantity = value; }
        [NotMapped]
        public double Weight { get => _weight; set => _weight = value; }
        [NotMapped]
        public Guid Id { get => id; set => id = value; }
        [NotMapped]
        public double Price { get => _price; set => _price = value; }
        [NotMapped]
        public string Category { get => category; set => category = value; }
        [NotMapped]
        public int Rating { get => rating; set => rating = value; }
        [NotMapped]
        public string StoreName { get => _storeName; set => _storeName = value; }
        [NotMapped]
        public double Discount { get => discount; set => discount = value; }

        public Product Clone()
        {
            var p = new Product(_storeId, _name, _quantity, _weight, _price, category, _storeName);
            p.Discount = discount;
            p.Id = id;
            return p;
        }
    }
}
