using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.Statuses;

namespace TradingSystem.Business
{
    public class ProductHistoryData
    {
        public HashSet<PurchasedProduct> productId_quantity { get; set; }
        public Guid id { get; set; }
        public ProductHistoryData()
        {
            ProductId_quantity = new HashSet<PurchasedProduct>();
        }
        [NotMapped]
        public HashSet<PurchasedProduct> ProductId_quantity { get => productId_quantity; set => productId_quantity = value; }

        public void Add(Product product, int quantity)
        {
            ProductId_quantity.Add(new PurchasedProduct(product.Id, product.Price, product.Name, quantity));
        }
    }
}
