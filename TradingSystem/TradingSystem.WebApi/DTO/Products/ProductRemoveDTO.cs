using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class ProductRemoveDTO
    {
        public string? Username { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
    }
}
