using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        public StoreData(Store store) {
            this.Id = store.Id;
            this.Name = store.Name;
            Products = new List<ProductData>();
            foreach (Product p in store.Products)
            {
                Products.Add(new ProductData(p));
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductData> Products { get; set; }
    }
}
