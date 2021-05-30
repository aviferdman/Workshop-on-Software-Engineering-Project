using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.History
{
    public class StoreHistorySpecificDTO
    {
        public string? Username { get; set; }
        public Guid StoreId { get; set; }
        public string? PaymentId { get; set; }
    }
}
