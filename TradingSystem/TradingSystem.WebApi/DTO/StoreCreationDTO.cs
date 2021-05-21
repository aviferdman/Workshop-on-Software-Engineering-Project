using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class StoreCreationDTO
    {
        public string? Username { get; set; }
        public string? StoreName { get; set; }
        public Address? Address { get; set; }
        public CreditCard? CreditCard { get; set; }
    }
}
