using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.WebApi.DTO.Store.Permissions
{
    public class PermissionsUpdateDTO
    {
        public AppointmentDTO? Appointment { get; set; }
        public string[]? Permissions { get; set; }
    }
}
