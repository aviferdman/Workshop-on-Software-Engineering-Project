using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public class ProductsStatus
    {
        private Guid clientId;
        private Guid storeId;
        private Dictionary<Product, int> product_quantity;

        public ProductsStatus(Guid clientId, Guid storeId, Dictionary<Product, int> product_quantity)
        {
            this.ClientId = clientId;
            this.StoreId = storeId;
            this.Product_quantity = product_quantity;
        }

        public Guid ClientId { get => clientId; set => clientId = value; }
        public Guid StoreId { get => storeId; set => storeId = value; }
        public Dictionary<Product, int> Product_quantity { get => product_quantity; set => product_quantity = value; }
    }
}