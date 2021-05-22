using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO
{
    public class ProductDetailsDTO
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public string? Category { get; set; }

        public ProductData ToProductData()
        {
            return new ProductData
            (
                Name,
                Quantity,
                Weight,
                Price,
                Category
            );
        }
    }
}
