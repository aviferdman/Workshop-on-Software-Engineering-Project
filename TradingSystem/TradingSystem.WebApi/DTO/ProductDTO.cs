using System;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO
{
    public class ProductDTO
    {
        public string? Name { get; set; }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public string? Category { get; set; }
        public int Rating { get; set; }

        public static ProductDTO FromProductData(ProductData productData)
        {
            return new ProductDTO
            {
                Id = productData.pid,
                Name = productData._name,
                Category = productData.category,
                Quantity = productData._quantity,
                Price = productData._price,
                Weight = productData._weight,
                Rating = productData.rating,
            };
        }
    }
}
