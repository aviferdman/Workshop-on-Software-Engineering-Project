using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    class MarketService
    {
        private Market market;

        public void AddProduct(ProductData product, Guid storeID, String username)
        {
            market.AddProduct(product, storeID, username);
        }

        public void RemoveProduct(String productName, Guid storeID, String username)
        {
            market.RemoveProduct(productName, storeID, username);
        }

        public void EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            market.EditProduct(productName, details, storeID, username);
        }
    }
}
