using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class StoreInfoDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }
    }
}
