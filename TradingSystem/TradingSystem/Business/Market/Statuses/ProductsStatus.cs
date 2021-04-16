using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public class ProductsStatus
    {
        private string _username;
        private Guid storeId;
        private ProductHistoryData productHistories;

        public ProductsStatus(string username, Guid storeId, Dictionary<Product, int> product_quantity)
        {
            this.Username = username;
            this.StoreId = storeId;
            ProductHistories = new ProductHistoryData();
            foreach (var p_q in product_quantity)
            {
                ProductHistories.Add(p_q.Key, p_q.Value);
            }
        }

        public Guid StoreId { get => storeId; set => storeId = value; }
        public ProductHistoryData ProductHistories { get => productHistories; set => productHistories = value; }
        public string Username { get => _username; set => _username = value; }
    }
}