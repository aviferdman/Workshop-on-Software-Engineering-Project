using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Permissions
{
    public class AppointmentDTO
    {
        public string? Assigner { get; set; }
        public Guid StoreID { get; set; }
        public string? Assignee { get; set; }
    }
}
