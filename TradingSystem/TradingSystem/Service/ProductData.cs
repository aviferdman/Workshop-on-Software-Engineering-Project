using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ProductData
    {
        public String _name;
        public Guid pid;
        public int _quantity;
        public double _weight;
        public double _price;
        public string category;
        public int rating;

        public ProductData(Product p)
        {
            _price = p.Price;
            _name = p.Name;
            _weight = p.Weight;
            _quantity = p.Quantity;
            pid = p.Id;
            category = p.Category;
            rating = p.Rating;
        }
    }
}
