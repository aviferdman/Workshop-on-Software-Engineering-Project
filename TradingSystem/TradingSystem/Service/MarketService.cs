using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    class MarketService
    {
        private Market market;

        public String AddProduct(ProductData product, Guid storeID, String username)
        {
            return market.AddProduct(product, storeID, username);
        }

        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            return market.RemoveProduct(productName, storeID, username);
        }

        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            return market.EditProduct(productName, details, storeID, username);
        }
    }
}
