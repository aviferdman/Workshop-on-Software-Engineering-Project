using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoresController : ControllerBase
    {
        public StoresController(MarketUserService marketUserService, IFileProvider fileProvider)
        {
            MarketUserService = marketUserService;
            FileProvider = fileProvider;
        }

        public MarketUserService MarketUserService { get; }
        public IFileProvider FileProvider { get; }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StoreInfoDTO>>> MyStores([FromBody] string username)
        {
            JsonDocument? json = await ParseJsonFromFile(FileProvider, "StoresData.json");
            if (json is null)
            {
                return InternalServerError();
            }
            return Ok(json.RootElement.GetProperty("stores"));
            //ICollection<StoreData>? stores = MarketUserService.getUserStores(username);
            //if (stores is null)
            //{
            //    return InternalServerError();
            //}

            //return Ok(stores.Select(store => new StoreInfoDTO
            //{
            //    Id = store.Id,
            //    Name = store.Name,
            //}));
        }
    }
}
