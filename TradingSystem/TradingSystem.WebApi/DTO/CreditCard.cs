using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO
{
    public class CreditCard
    {
        public string? CardNumber { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public string? HolderName { get; set; }
        public string? Cvv { get; set; }
        public string? HolderId { get; set; }
    }
}
