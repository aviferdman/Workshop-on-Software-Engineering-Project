using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.StorePackage
{
    public class Bid
    {
        string _username;
        Guid _productId;
        double _price;

        public Bid(string username, Guid productId, double price)
        {
            this.Username = username;
            this.ProductId = productId;
            this.Price = price;
        }

        public string Username { get => _username; set => _username = value; }
        public Guid ProductId { get => _productId; set => _productId = value; }
        public double Price { get => _price; set => _price = value; }
    }
}
