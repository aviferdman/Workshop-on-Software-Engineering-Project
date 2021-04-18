using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class StoreData
    {
        private Guid _id;
        private string name;
        private List<ProductData> products;
        public StoreData(Store store)
        {
            this._id = store.Id;
            this.name = store.Name;
            products = new List<ProductData>();
            foreach(Product p in store.Products.Values)
            {
                products.Add(new ProductData(p));
            }
        }
    }
}
