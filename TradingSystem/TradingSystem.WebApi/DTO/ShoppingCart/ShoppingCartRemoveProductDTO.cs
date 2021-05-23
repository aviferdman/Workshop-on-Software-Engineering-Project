using System;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartRemoveProductDTO
    {
        public string? Username { get; set; }
        public Guid ProductId { get; set; }
    }
}
