using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.Store.Permissions;

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

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ShoppingBasketHistoryDTO>>> Mine([FromBody] UsernameDTO usernameDTO)
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

            return Ok(result.Select(ShoppingBasketHistoryDTO.FromHistoryData));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ShoppingBasketHistoryDTO>>> OfStore([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            if (string.IsNullOrWhiteSpace(storeInfoActionDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            ICollection<HistoryData>? result = await MarketStoreGeneralService.GetStoreHistory(storeInfoActionDTO.Username, storeInfoActionDTO.StoreID);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(ShoppingBasketHistoryDTO.FromHistoryData));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ShoppingBasketHistoryDTO>>> OfAll([FromBody] UsernameDTO usernameDTO)
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

            return Ok(result.Select(ShoppingBasketHistoryDTO.FromHistoryData));
        }
    }
}
