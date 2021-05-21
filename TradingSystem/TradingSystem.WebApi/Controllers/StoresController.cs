using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using TradingSystem.Business.Market;
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
            MarketProductsService marketProductsService
        )
        {
            MarketUserService = marketUserService;
            MarketStoreGeneralService = marketStoreGeneralService;
            MarketProductsService = marketProductsService;
        }

        public MarketUserService MarketUserService { get; }
        public MarketStoreGeneralService MarketStoreGeneralService { get; }
        public MarketProductsService MarketProductsService { get; }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StoreInfoDTO>>> MyStores([FromBody] string username)
        {
            ICollection<StoreData>? stores = await MarketUserService.getUserStores(username);
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
        public async Task<ActionResult<StoreRefDTO>> Create([FromBody] StoreCreationDTO storeCreationDTO)
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

            StoreData storeData = await MarketStoreGeneralService.CreateStoreAsync
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

        [HttpPost]
        public async Task<ActionResult<ProductRefDTO>> AddProduct([FromBody] ProductCreationDTO productCreationDTO)
        {
            object?[] values =
            {
                productCreationDTO.Username,
                productCreationDTO.Product?.Name,
                productCreationDTO.Product?.Category,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            Result<Product> productResult = await MarketProductsService.AddProduct
            (
                new ProductData
                (
                    productCreationDTO.Product!.Name,
                    productCreationDTO.Product!.Quantity,
                    productCreationDTO.Product!.Weight,
                    productCreationDTO.Product!.Price,
                    productCreationDTO.Product!.Category
                ),
                productCreationDTO.StoreId,
                productCreationDTO.Username
            );

            if (productResult == null)
            {
                return InternalServerError();
            }
            if (productResult.IsErr)
            {
                return InternalServerError(string.IsNullOrWhiteSpace(productResult.Mess) ? null : productResult.Mess);
            }

            Product product = productResult.Ret;
            return new ProductRefDTO
            {
                Id = product.Id,
                Name = product.Name,
            };
        }
    }
}
