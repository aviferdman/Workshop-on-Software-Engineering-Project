using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.StorePackage
{
    public enum BidStatus
    {
        Accept,
        Deny,
        Negotiate
    }
    public class Bid
    {
        Guid id;
        string _username;
        Guid _storeId;
        Guid _productId;
        double _price;
        BidStatus status;

        public Bid(string username, Guid storeId, Guid productId, double price)
        {
            this.Id = Guid.NewGuid();
            this.Username = username;
            this.StoreId = storeId;
            this.ProductId = productId;
            this.Price = price;
            this.Status = BidStatus.Negotiate;
        }

        public string Username { get => _username; set => _username = value; }
        public Guid ProductId { get => _productId; set => _productId = value; }
        public double Price { get => _price; set => _price = value; }
        public BidStatus Status { get => status; set => status = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public Guid Id { get => id; set => id = value; }
    }
}
