using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business
{
    public class ProductHistoryData
    {
        IDictionary<Guid, int> productId_quantity;
        public ProductHistoryData()
        {
            ProductId_quantity = new Dictionary<Guid, int>();
        }

        public IDictionary<Guid, int> ProductId_quantity { get => productId_quantity; set => productId_quantity = value; }

        public void Add(Product product, int quantity)
        {
            ProductId_quantity.Add(product.Id, quantity);
        }
    }
}
