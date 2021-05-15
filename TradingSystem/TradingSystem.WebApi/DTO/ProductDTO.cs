using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
