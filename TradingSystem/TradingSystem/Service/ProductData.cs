using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ProductData
    {
        public String _name;
        public Guid pid;
        public Guid _storeId;
        public int _quantity;
        public double _weight;
        public double _price;
        public String category;
        public int rating;
        private string storeName;

        public string StoreName { get => storeName; set => storeName = value; }

        public ProductData(Guid storeId, String name, int quantity, double weight, double price, String category, string storeName = "")
        {
            this._storeId = storeId;
            this._name = name;
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this.category = category;
            rating = -1;
            this.StoreName = storeName;
        }

        public ProductData(String name, int quantity, double weight, double price, String category, string storeName = "")
        {
            this._name = name;
            this._quantity = quantity;
            this._weight = weight;
            this._price = price;
            this.category = category;
            rating = -1;
            this.StoreName = storeName;
        }

        public ProductData(Product p)
        {
            _storeId = p._storeId;
            _price = p.Price;
            _name = p.Name;
            _weight = p.Weight;
            _quantity = p.Quantity;
            pid = p.Id;
            category = p.Category;
            rating = p.Rating;
            StoreName = p.StoreName;
        }
    }
}
