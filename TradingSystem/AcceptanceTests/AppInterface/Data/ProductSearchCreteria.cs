using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductSearchCreteria
    {
        public ProductSearchCreteria(string keywords)
        {
            Keywords = keywords;
            Category = null;
            PriceRange_Low = -1;
            PriceRange_High = -1;
            ProductRating = -1;
            ShopRating = -1;
        }

        public string Keywords { get; }
        public string? Category { get; set; }
        public decimal PriceRange_Low { get; set; }
        public decimal PriceRange_High { get; set; }
        public decimal ProductRating { get; set; }
        public decimal ShopRating { get; set; }
    }
}
