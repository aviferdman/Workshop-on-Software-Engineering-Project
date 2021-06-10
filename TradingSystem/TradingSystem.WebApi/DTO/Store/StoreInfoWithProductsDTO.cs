using System;
using System.Collections.Generic;

namespace TradingSystem.WebApi.DTO
{
    public class StoreInfoWithProductsDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }
    }
}
