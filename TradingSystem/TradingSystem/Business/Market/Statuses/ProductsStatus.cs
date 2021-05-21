using System;
using System.Collections.Generic;
using TradingSystem.Business.Market.UserPackage;

namespace TradingSystem.Business.Market
{
    public class ProductsStatus
    {
        private string _username;
        private Guid storeId;
        private ProductHistoryData productHistories;

        public ProductsStatus(string username, Guid storeId, HashSet<ProductInCart> product_quantity)
        {
            this.Username = username;
            this.StoreId = storeId;
            ProductHistories = new ProductHistoryData();
            foreach (var p_q in product_quantity)
            {
                ProductHistories.Add(p_q.product, p_q.quantity);
            }
        }

        public Guid StoreId { get => storeId; set => storeId = value; }
        public ProductHistoryData ProductHistories { get => productHistories; set => productHistories = value; }
        public string Username { get => _username; set => _username = value; }
    }
}