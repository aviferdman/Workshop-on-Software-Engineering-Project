using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Permissions
{
    public class StoreInfoActionDTO
    {
        public string? Username { get; set; }
        public Guid StoreID { get; set; }
    }
}
