using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public class ProductsStatus
    {
        private Guid clientId;
        private Guid storeId;
        private ProductHistoryData productHistories;

        public ProductsStatus(Guid clientId, Guid storeId, Dictionary<Product, int> product_quantity)
        {
            this.ClientId = clientId;
            this.StoreId = storeId;
            ProductHistories = new ProductHistoryData();
            foreach (var p_q in product_quantity)
            {
                ProductHistories.Add(p_q.Key, p_q.Value);
            }
        }

        public Guid ClientId { get => clientId; set => clientId = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public ProductHistoryData ProductHistories { get => productHistories; set => productHistories = value; }
    }
}