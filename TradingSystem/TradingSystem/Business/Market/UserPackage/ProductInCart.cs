using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.UserPackage
{
    public class ProductInCart
    {
        public Product product { get; set; }
        public int quantity { get; set; }

        public ProductInCart(Product product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }
}
