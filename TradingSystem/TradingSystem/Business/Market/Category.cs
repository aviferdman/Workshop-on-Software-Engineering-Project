using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Category
    {
        List<Product> products;
        string name;

        public List<Product> Products { get => products; set => products = value; }
        public string Name { get => name; set => name = value; }

        public Category(string name)
        {
            this.name = name;
            products = new List<Product>();
        }
        
        public void addProduct(Product p)
        {
            if(!products.Contains(p))
                products.Add(p);
        }
        public List<Product> getAllProducts(string keyword, int price_range_low, int price_range_high, int rating)
        {
            List<Product> pros = new List<Product>();
            foreach (Product p in products)
            {
                if ((p.Name != null && p.Name.Contains(keyword)) || (name.Contains(keyword)))
                {
                    if (price_range_low != -1 && price_range_low > p.Price)
                        continue;
                    if (price_range_high != -1 && price_range_high < p.Price)
                        continue;
                    if (rating != -1 && rating != p.Rating)
                        continue;
                    pros.Add(p);
                }
            }
            return pros;
        }
    }
}
