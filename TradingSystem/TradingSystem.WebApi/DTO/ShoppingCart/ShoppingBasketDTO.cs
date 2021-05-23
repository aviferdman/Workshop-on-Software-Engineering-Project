using System;
using System.Collections.Generic;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingBasketDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<ShoppingBasketProductDTO>? Products { get; set; }
    }
}
