using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketProductsService
    {
        private static readonly Lazy<MarketProductsService> instanceLazy = new Lazy<MarketProductsService>(() => new MarketProductsService(), true);

        private readonly MarketStores marketStores;

        private MarketProductsService()
        {
            marketStores = MarketStores.Instance;
        }

        public static MarketProductsService Instance => instanceLazy.Value;

        //"Product added"
        public Result<Product> AddProduct(ProductData product, Guid storeID, string username)
        {
            return marketStores.AddProduct(product, storeID, username);
        }

        //"Product removed"
        public string RemoveProduct(Guid productID, Guid storeID, string username)
        {
            return marketStores.RemoveProduct(productID, storeID, username);
        }
        //"Product edited"
        public string EditProduct(Guid productID, ProductData details, Guid storeID, string username)
        {
            return marketStores.EditProduct(productID, details, storeID, username);
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
