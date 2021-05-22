using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartAddProductDTO
    {
        public string? Username { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
