using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market.StorePackage;

namespace TradingSystem.WebApi.DTO.Store.Permissions
{
    public class WorkerDetailsDTO
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public IEnumerable<string>? Permissions { get; set; }

        public static WorkerDetailsDTO FromWorkerDetails(WorkerDetails workerDetails)
        {
            return new WorkerDetailsDTO
            {
                Username = workerDetails.Username,
                Role = workerDetails.Position,
                Permissions = workerDetails.Permissions,
            };
        }
    }
}
