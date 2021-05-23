using System;

namespace TradingSystem.WebApi.DTO
{
    public class ProductCreateDTO
    {
        public string? Username { get; set; }
        public Guid StoreId { get; set; }
        public ProductDetailsDTO? ProductDetails { get; set; }
    }
}
