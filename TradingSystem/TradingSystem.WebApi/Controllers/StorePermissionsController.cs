using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO.Store.Permissions;

using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/stores/permissions/[action]")]
    public class StorePermissionsController : ControllerBase
    {
        public StorePermissionsController
        (
            MarketStorePermissionsManagementService marketStorePermissionsManagementService
        )
        {
            MarketStorePermissionsManagementService = marketStorePermissionsManagementService;
        }

        public MarketStorePermissionsManagementService MarketStorePermissionsManagementService { get; }

        [HttpPost]
        public async Task<ActionResult<WorkerDetailsDTO>> MyPermissions([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            object?[] values =
            {
                storeInfoActionDTO.Username,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing username value");
            }

            Result<WorkerDetails>? result = await MarketStorePermissionsManagementService.GetPrems
            (
                storeInfoActionDTO.StoreID,
                storeInfoActionDTO.Username
            );

            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                if (result.Mess == "user doesnt exist")
                {
                    return Ok(new WorkerDetailsDTO
                    {
                        Username = storeInfoActionDTO.Username,
                        Role = "guest",
                        Permissions = Enumerable.Empty<string>(),
                    });
                }
                return InternalServerError(result.Mess);
            }

            return Ok(WorkerDetailsDTO.FromWorkerDetails(result.Ret));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<WorkerDetailsDTO>>> WorkersDetails([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            object?[] values =
            {
                storeInfoActionDTO.Username,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing username value");
            }

            ICollection<WorkerDetails>? result = await MarketStorePermissionsManagementService.GetInfo
            (
                storeInfoActionDTO.StoreID,
                storeInfoActionDTO.Username
            );
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(WorkerDetailsDTO.FromWorkerDetails));
        }

        [HttpPost]
        public async Task<ActionResult<WorkerDetailsDTO>> WorkerSpecificDetails([FromBody] AppointmentDTO appointmentDTO)
        {
            object?[] values =
            {
                appointmentDTO.Assignee,
                appointmentDTO.Assigner,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            WorkerDetails? result = await MarketStorePermissionsManagementService.GetInfoSpecific
            (
                appointmentDTO.StoreID,
                appointmentDTO.Assignee,
                appointmentDTO.Assigner
            );
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(WorkerDetailsDTO.FromWorkerDetails(result));
        }

        [HttpPost]
        public async Task<ActionResult> AppointOwner([FromBody] AppointmentDTO appointmentDTO)
        {
            object?[] values =
            {
                appointmentDTO.Assignee,
                appointmentDTO.Assigner,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string result = await MarketStorePermissionsManagementService.MakeOwnerAsync
            (
                appointmentDTO.Assignee,
                appointmentDTO.StoreID,
                appointmentDTO.Assigner
            );
            if (result != "Success")
            {
                return InternalServerError(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AppointManager([FromBody] AppointmentDTO appointmentDTO)
        {
            object?[] values =
            {
                appointmentDTO.Assignee,
                appointmentDTO.Assigner,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string result = await MarketStorePermissionsManagementService.MakeManagerAsync
            (
                appointmentDTO.Assignee,
                appointmentDTO.StoreID,
                appointmentDTO.Assigner
            );
            if (result != "Success")
            {
                return InternalServerError(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateManagerPermissions([FromBody] PermissionsUpdateDTO permissionsUpdateDTO)
        {
            object?[] values =
            {
                permissionsUpdateDTO.Appointment?.Assignee,
                permissionsUpdateDTO.Appointment?.Assigner,
                permissionsUpdateDTO.Permissions,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            var permissions = new List<Permission>(permissionsUpdateDTO.Permissions!.Length);
            foreach (string? permissonStr in permissionsUpdateDTO.Permissions!)
            {
                Permission permission;
                if (!Enum.TryParse(permissonStr, out permission))
                {
                    return BadRequest($"Invalid permission '{permissonStr}'");
                }
                permissions.Add(permission);
            }

            string result = await MarketStorePermissionsManagementService.DefineManagerPermissionsAsync
            (
                permissionsUpdateDTO.Appointment!.Assignee,
                permissionsUpdateDTO.Appointment!.StoreID,
                permissionsUpdateDTO.Appointment!.Assigner,
                permissions
            );
            if (result != "Success")
            {
                return InternalServerError(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> RemoveManager([FromBody] AppointmentDTO appointmentDTO)
        {
            object?[] values =
            {
                appointmentDTO.Assignee,
                appointmentDTO.Assigner,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string result = await MarketStorePermissionsManagementService.RemoveManagerAsync
            (
                appointmentDTO.Assignee,
                appointmentDTO.StoreID,
                appointmentDTO.Assigner
            );
            if (result != "success")
            {
                return InternalServerError(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> RemoveOwner([FromBody] AppointmentDTO appointmentDTO)
        {
            object?[] values =
            {
                appointmentDTO.Assignee,
                appointmentDTO.Assigner,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string result = await MarketStorePermissionsManagementService.RemoveOwnerAsync
            (
                appointmentDTO.Assignee,
                appointmentDTO.StoreID,
                appointmentDTO.Assigner
            );
            if (result != "success")
            {
                return InternalServerError(result);
            }

            return Ok();
        }
    }
}
