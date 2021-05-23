using System;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartEditProductDTO
    {
        public string? Username { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
