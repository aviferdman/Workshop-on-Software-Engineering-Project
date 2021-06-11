using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.Store;

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
        public async Task<ActionResult<IEnumerable<StoreRefDTO>>> Search([FromBody] StoreNameDTO storeNameDTO)
        {
            if (string.IsNullOrWhiteSpace(storeNameDTO.StoreName))
            {
                return BadRequest("Invalid store name");
            }

            ICollection<StoreData>? stores = await MarketStoreGeneralService.FindStoresByName(storeNameDTO.StoreName);
            if (stores is null)
            {
                return InternalServerError();
            }

            return Ok(stores.Select(store => new StoreRefDTO
            {
                Id = store.Id,
                Name = store.Name,
            }));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StoreRefDTO>>> MyStores([FromBody] string username)
        {
            ICollection<StoreData>? stores = await MarketUserService.getUserStores(username);
            if (stores is null)
            {
                return InternalServerError();
            }

            return Ok(stores.Select(store => new StoreRefDTO
            {
                Id = store.Id,
                Name = store.Name,
            }));
        }

        [HttpGet]
        public async Task<ActionResult<StoreRefDTO>> Info([FromQuery] Guid storeId)
        {
            StoreData? store = await MarketStoreGeneralService.getStoreById(storeId);
            if (store == null)
            {
                return InternalServerError();
            }

            return Ok(new StoreRefDTO
            {
                Id = store.Id,
                Name = store.Name,
            });
        }

        [HttpGet]
        public async Task<ActionResult<StoreInfoWithProductsDTO>> InfoWithProducts([FromQuery] Guid storeId)
        {
            StoreData? store = await MarketStoreGeneralService.getStoreById(storeId);
            if (store == null)
            {
                return InternalServerError();
            }

            return Ok(new StoreInfoWithProductsDTO
            {
                Id = store.Id,
                Name = store.Name,
                Products = store.Products.Select(ProductDTO.FromProductData),
            });
        }

        [HttpPost]
        public async Task<ActionResult<StoreRefDTO>> Create([FromBody] StoreCreateDTO storeCreationDTO)
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
        public async Task<ActionResult<ProductRefDTO>> AddProduct([FromBody] ProductCreateDTO productCreationDTO)
        {
            object?[] values =
            {
                productCreationDTO.Username,
                productCreationDTO.ProductDetails?.Name,
                productCreationDTO.ProductDetails?.Category,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            Result<Product> productResult = await MarketProductsService.AddProduct
            (
                productCreationDTO.ProductDetails!.ToProductData(),
                productCreationDTO.StoreId,
                productCreationDTO.Username
            );

            if (productResult == null)
            {
                return InternalServerError();
            }
            if (productResult.IsErr)
            {
                return string.IsNullOrWhiteSpace(productResult.Mess) ?
                    InternalServerError() :
                    InternalServerError(productResult.Mess);
            }

            Product product = productResult.Ret;
            return new ProductRefDTO
            {
                Id = product.Id,
                Name = product.Name,
            };
        }

        [HttpPost]
        public async Task<ActionResult> EditProduct([FromBody] ProductEditDTO productEditDTO)
        {
            object?[] values =
            {
                productEditDTO.Username,
                productEditDTO.ProductDetails?.Name,
                productEditDTO.ProductDetails?.Category,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string result = await MarketProductsService.EditProductAsync
            (
                productEditDTO.ProductId,
                productEditDTO.ProductDetails!.ToProductData(),
                productEditDTO.StoreId,
                productEditDTO.Username
            );

            if (string.IsNullOrWhiteSpace(result))
            {
                return InternalServerError();
            }
            if (result != "Product edited")
            {
                return InternalServerError(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> RemoveProduct([FromBody] ProductRemoveDTO productRemoveDTO)
        {
            object?[] values =
            {
                productRemoveDTO.Username,
            };
            if (values.Contains(null))
            {
                return BadRequest("Missing parameter values");
            }

            string? result = await MarketProductsService.RemoveProductAsync
            (
                productRemoveDTO.ProductId,
                productRemoveDTO.StoreId,
                productRemoveDTO.Username
            );

            if (string.IsNullOrWhiteSpace(result))
            {
                return InternalServerError();
            }
            if (result != "Product removed")
            {
                return InternalServerError(result);
            }

            return Ok();
        }
    }
}
