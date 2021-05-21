using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.Statuses
{
    public class PurchasedProduct
    {
        public PurchasedProduct(Guid id, double price, string name, int quantity)
        {
            this.id = id;
            this.price = price;
            this.name = name;
            this.quantity = quantity;
        }

        public Guid id { get; set; }
        public double price { get; set; }
        public string name { get; set; }

        public int quantity { get; set; }


    }
}
