using System.Collections.Generic;
using System.Linq;
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
        public StoresController
        (
            MarketUserService marketUserService,
            MarketStoreGeneralService marketStoreGeneralService,
            IFileProvider fileProvider
        )
        {
            MarketUserService = marketUserService;
            MarketStoreGeneralService = marketStoreGeneralService;
            FileProvider = fileProvider;
        }

        public MarketUserService MarketUserService { get; }
        public MarketStoreGeneralService MarketStoreGeneralService { get; }
        public IFileProvider FileProvider { get; }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StoreInfoDTO>>> MyStores([FromBody] string username)
        {
            ICollection<StoreData>? stores = MarketUserService.getUserStores(username);
            if (stores is null)
            {
                return InternalServerError();
            }

            return Ok(stores.Select(store => new StoreInfoDTO
            {
                Id = store.Id,
                Name = store.Name,
            }));
        }

        [HttpPost]
        public ActionResult<StoreRefDTO> Create([FromBody] StoreCreationDTO storeCreationDTO)
        {
            object?[] values =
            {
                storeCreationDTO.StoreName,
                storeCreationDTO.Username,
                storeCreationDTO.CreditCard?.CardNumber,
                storeCreationDTO.CreditCard?.Month,
                storeCreationDTO.CreditCard?.Year,
                storeCreationDTO.CreditCard?.HolderName,
                storeCreationDTO.CreditCard?.Cvv,
                storeCreationDTO.CreditCard?.HolderId,
                storeCreationDTO.Address?.State,
                storeCreationDTO.Address?.City,
                storeCreationDTO.Address?.Street,
                storeCreationDTO.Address?.ApartmentNumber,
                storeCreationDTO.Address?.ZipCode,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            StoreData storeData = MarketStoreGeneralService.CreateStore
            (
                storeCreationDTO.StoreName,
                storeCreationDTO.Username,
                storeCreationDTO.CreditCard?.CardNumber,
                storeCreationDTO.CreditCard?.Month,
                storeCreationDTO.CreditCard?.Year,
                storeCreationDTO.CreditCard?.HolderName,
                storeCreationDTO.CreditCard?.Cvv,
                storeCreationDTO.CreditCard?.HolderId,
                storeCreationDTO.Address?.State,
                storeCreationDTO.Address?.City,
                storeCreationDTO.Address?.Street,
                storeCreationDTO.Address?.ApartmentNumber,
                storeCreationDTO.Address?.ZipCode
            );
            if (storeData is null)
            {
                return InternalServerError();
            }

            return Ok(new StoreRefDTO
            {
                Id = storeData.Id,
                Name = storeData.Name,
            });
        }
    }
}
