using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ICollection<string> getAllCategories()
        {
            return marketStores.getAllCategories();
        }
        //"Product added"
        public async Task<Result<Product>> AddProduct(ProductData product, Guid storeID, string username)
        {
            return await marketStores.AddProduct(product, storeID, username);
        }

        //"Product removed"
        public async Task<string> RemoveProductAsync(Guid productID, Guid storeID, string username)
        {
            return await marketStores.RemoveProduct(productID, storeID, username);
        }
        //"Product edited"
        public async Task<string> EditProductAsync(Guid productID, ProductData details, Guid storeID, string username)
        {
            return await marketStores.EditProduct(productID, details, storeID, username);
        }

        public async Task<ICollection<ProductData>> FindProductsByStoresAsync(string name)
        {
            ICollection<Store> stores = await marketStores.GetStoresByName(name);
            ICollection<ProductData> products = new LinkedList<ProductData>();
            foreach (Store s in stores)
            {
                foreach (Product p in s.Products)
                {
                    products.Add(new ProductData(p));
                }
            }
            return products;
        }

        public async Task<ICollection<ProductData>> FindProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new ProductData[] { };
            }

            ICollection<Product> pro = await marketStores.findProducts(keyword, price_range_low, price_range_high, rating, category);
            ICollection<ProductData> products = new LinkedList<ProductData>();
            foreach (Product p in pro)
            {
                products.Add(new ProductData(p));
            }
            return products;
        }
    }
}
