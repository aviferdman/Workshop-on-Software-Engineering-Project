using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        private string name;
        private Guid id;
        private List<ProductData> products;
        public StoreData(Store store) { 
            this.id = store.Id;
            this.name = store.Name;
            products = new List<ProductData>();
            foreach(Product p in store.Products.Values)
            {
                products.Add(new ProductData(p));
            }
        }

        public Guid Id { get => id; set => id = value; }
    }
}
