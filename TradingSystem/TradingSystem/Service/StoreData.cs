using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        private List<ProductData> products;
        public StoreData(Store store) { 
            this.Id = store.Id;
            this.Name = store.Name;
            products = new List<ProductData>();
            foreach(Product p in store.Products.Values)
            {
                products.Add(new ProductData(p));
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
