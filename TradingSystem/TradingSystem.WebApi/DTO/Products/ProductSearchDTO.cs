using System;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO.Products
{
    public class ProductSearchDTO
    {
        public string? Name { get; set; }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public string? Category { get; set; }
        public string? StoreName { get; set; }
        public int Rating { get; set; }

        public static ProductSearchDTO FromProductData(ProductData productData)
        {
            return new ProductSearchDTO
            {
                Id = productData.pid,
                Name = productData._name,
                Category = productData.category,
                Quantity = productData._quantity,
                Price = productData._price,
                Weight = productData._weight,
                StoreName = productData.StoreName,
                Rating = productData.rating,
            };
        }
    }
}
