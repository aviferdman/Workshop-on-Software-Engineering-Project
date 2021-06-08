using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.DiscountPackage
{
    public class DiscountOfProducts
    {
        private double discount;
        private IDictionary<Guid, double> products;

        public double Discount { get => discount; set => discount = value; }
        public IDictionary<Guid, double> Products { get => products; set => products = value; }

        public DiscountOfProducts()
        {
            this.Products = new Dictionary<Guid, double>();
        }

        public void AddProduct(Guid productId, double price)
        {
            this.products.Add(productId, price);
        }
        public void IncrementDiscount(double val)
        {
            Discount += val;
        }
        public DiscountOfProducts AddDiscounts(double otherDiscount, IDictionary<Guid, double> otherProducts)
        {
            var ret = new DiscountOfProducts();
            ret.Discount = discount + otherDiscount;
            foreach (var p in products)
            {
                ret.Products.Add(p.Key, p.Value + otherProducts[p.Key]);
            }
            return ret;
        }
    }
}
