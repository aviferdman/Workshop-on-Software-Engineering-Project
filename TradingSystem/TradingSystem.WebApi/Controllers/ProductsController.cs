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
    public class ProductsController : ControllerBase
    {
        public ProductsController(MarketProductsService marketProductsService)
        {
            MarketProductsService = marketProductsService;
        }

        public MarketProductsService MarketProductsService { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Search([FromQuery] ProductSearchCreteria searchCreteria)
        {
            if (searchCreteria == null)
            {
                return BadRequest();
            }

            IEnumerable<ProductData> products = await MarketProductsService.FindProducts
            (
                searchCreteria.Keywords,
                searchCreteria.PriceRange_Low,
                searchCreteria.PriceRange_High,
                searchCreteria.ProductRating,
                searchCreteria.Category
            );
            if (products == null)
            {
                return InternalServerError();
            }

            return Ok(products.Select(ProductDTO.FromProductData));
        }
    }
}
