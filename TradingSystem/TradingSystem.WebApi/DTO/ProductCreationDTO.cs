using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class ProductCreationDTO
    {
        public string? Username { get; set; }
        public Guid StoreId { get; set; }
        public ProductDTO? Product { get; set; }
    }
}
