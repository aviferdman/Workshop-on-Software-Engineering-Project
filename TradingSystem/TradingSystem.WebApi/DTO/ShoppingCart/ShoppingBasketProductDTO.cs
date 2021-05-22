using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingBasketProductDTO
    {
        public string? Name { get; set; }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public string? Category { get; set; }
        public int Rating { get; set; }

        public static ShoppingBasketProductDTO FromProductData(ProductData productData, int quantity)
        {
            return new ShoppingBasketProductDTO
            {
                Id = productData.pid,
                Name = productData._name,
                Category = productData.category,
                Quantity = quantity,
                Price = productData._price,
                Weight = productData._weight,
                Rating = productData.rating,
            };
        }
    }
}
