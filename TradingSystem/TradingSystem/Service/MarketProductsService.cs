using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketProductsService
    {
        private readonly MarketStores marketStores;

        public MarketProductsService()
        {
            marketStores = MarketStores.Instance;
        }

        //"Product added"
        public string AddProduct(ProductData product, Guid storeID, string username)
        {
            return marketStores.AddProduct(product, storeID, username);
        }

        //"Product removed"
        public string RemoveProduct(string productName, Guid storeID, string username)
        {
            return marketStores.RemoveProduct(productName, storeID, username);
        }
        //"Product edited"
        public string EditProduct(string productName, ProductData details, Guid storeID, string username)
        {
            return marketStores.EditProduct(productName, details, storeID, username);
        }

        public ICollection<ProductData> FindProductsByStores(string name)
        {
            ICollection<Store> stores = marketStores.GetStoresByName(name);
            ICollection<ProductData> products = new LinkedList<ProductData>();
            foreach (Store s in stores)
            {
                foreach (Product p in s.Products.Values)
                {
                    products.Add(new ProductData(p));
                }
            }
            return products;
        }

        public ICollection<ProductData> FindProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            ICollection<Product> pro = marketStores.findProducts(keyword, price_range_low, price_range_high, rating, category);
            ICollection<ProductData> products = new LinkedList<ProductData>();
            foreach (Product p in pro)
            {
                products.Add(new ProductData(p));
            }
            return products;
        }
    }
}
