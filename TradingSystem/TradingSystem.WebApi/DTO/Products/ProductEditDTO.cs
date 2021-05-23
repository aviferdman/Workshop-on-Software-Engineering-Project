using System;

namespace TradingSystem.WebApi.DTO
{
    public class ProductEditDTO
    {
        public string? Username { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDetailsDTO? ProductDetails { get; set; }
    }
}
