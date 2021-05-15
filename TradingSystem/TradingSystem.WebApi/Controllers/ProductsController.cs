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
    public class ProductsController : ControllerBase
    {
        public ProductsController(MarketProductsService marketProductsService, IFileProvider fileProvider)
        {
            MarketProductsService = marketProductsService;
            FileProvider = fileProvider;
        }

        public MarketProductsService MarketProductsService { get; }
        public IFileProvider FileProvider { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Search([FromQuery] ProductSearchCreteria searchCreteria)
        {
            JsonDocument? json = await ParseJsonFromFile(FileProvider, "productData.json");
            if (json is null)
            {
                return InternalServerError();
            }
            return Ok(json.RootElement.GetProperty("products"));
            //if (searchCreteria == null)
            //{
            //    return BadRequest();
            //}

            //IEnumerable<ProductData> products = MarketProductsService.FindProducts
            //(
            //    searchCreteria.Keywords,
            //    searchCreteria.PriceRange_Low,
            //    searchCreteria.PriceRange_High,
            //    searchCreteria.ProductRating,
            //    searchCreteria.Category
            //);
            //if (products == null)
            //{
            //    return this.InternalServerError();
            //}

            //return Ok(products.Select(ProductDTO.FromProductData));
        }
    }
}
