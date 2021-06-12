using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.History;
using TradingSystem.WebApi.DTO.Store;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HistoryController : ControllerBase
    {
        public HistoryController
        (
            MarketGeneralService marketGeneralService,
            MarketStoreGeneralService marketStoreGeneralService,
            MarketUserService marketUserService
        )
        {
            MarketGeneralService = marketGeneralService;
            MarketStoreGeneralService = marketStoreGeneralService;
            MarketUserService = marketUserService;
        }

        public MarketGeneralService MarketGeneralService { get; }
        public MarketStoreGeneralService MarketStoreGeneralService { get; }
        public MarketUserService MarketUserService { get; }

        private ActionResult<HistoryRecordDTO> SingleHistory(string paymentId, ICollection<HistoryData> result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            HistoryData? historyData = result
                .Where(x => x.Payments.PaymentId == paymentId)
                .FirstOrDefault();
            if (historyData == null)
            {
                return InternalServerError("History with the specified payment ID does not exist");
            }
            return Ok(HistoryRecordDTO.FromHistoryData(historyData));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HistoryRecordDTO>>> Mine([FromBody] UsernameDTO usernameDTO)
        {
            if (string.IsNullOrWhiteSpace(usernameDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            ICollection<HistoryData>? result = await MarketUserService.GetUserHistory(usernameDTO.Username);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(HistoryRecordDTO.FromHistoryData));
        }

        [HttpPost]
        public async Task<ActionResult<HistoryRecordDTO>> MineSpecific([FromBody] InfoActionDTO<string> historyInfoSpecificDTO)
        {
            if (string.IsNullOrWhiteSpace(historyInfoSpecificDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (string.IsNullOrWhiteSpace(historyInfoSpecificDTO.Key))
            {
                return BadRequest("Invalid payment ID");
            }

            ICollection<HistoryData>? result = await MarketUserService.GetUserHistory(historyInfoSpecificDTO.Username);
            if (result == null)
            {
                return InternalServerError();
            }

            return SingleHistory(historyInfoSpecificDTO.Key, result);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HistoryRecordDTO>>> OfStore([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            if (string.IsNullOrWhiteSpace(storeInfoActionDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            ICollection<HistoryData>? result = await MarketStoreGeneralService.GetStoreHistory(storeInfoActionDTO.Username, storeInfoActionDTO.StoreId);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(HistoryRecordDTO.FromHistoryData));
        }

        [HttpPost]
        public async Task<ActionResult<HistoryRecordDTO>> OfStoreSpecific([FromBody] StoreHistorySpecificDTO storeHistorySpecificDTO)
        {
            if (string.IsNullOrWhiteSpace(storeHistorySpecificDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (string.IsNullOrWhiteSpace(storeHistorySpecificDTO.PaymentId))
            {
                return BadRequest("Invalid payment ID");
            }

            ICollection<HistoryData>? result = await MarketStoreGeneralService.GetStoreHistory(storeHistorySpecificDTO.Username, storeHistorySpecificDTO.StoreId);
            if (result == null)
            {
                return InternalServerError();
            }

            return SingleHistory(storeHistorySpecificDTO.PaymentId, result);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HistoryRecordDTO>>> OfAll([FromBody] UsernameDTO usernameDTO)
        {
            if (string.IsNullOrWhiteSpace(usernameDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            ICollection<HistoryData>? result = await MarketGeneralService.GetAllHistoryAsync(usernameDTO.Username);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(HistoryRecordDTO.FromHistoryData));
        }

        [HttpPost]
        public async Task<ActionResult<HistoryRecordDTO>> OfAllSpecific([FromBody] InfoActionDTO<string> historyInfoSpecificDTO)
        {
            if (string.IsNullOrWhiteSpace(historyInfoSpecificDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (string.IsNullOrWhiteSpace(historyInfoSpecificDTO.Key))
            {
                return BadRequest("Invalid payment ID");
            }

            ICollection<HistoryData>? result = await MarketGeneralService.GetAllHistoryAsync(historyInfoSpecificDTO.Username);
            if (result == null)
            {
                return InternalServerError();
            }

            return SingleHistory(historyInfoSpecificDTO.Key, result);
        }
    }
}
