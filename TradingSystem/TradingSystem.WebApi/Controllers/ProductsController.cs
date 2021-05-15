using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using TradingSystem.Business.Market;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using static TradingSystem.WebApi.ControllerExtensions;

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
        public ActionResult<IEnumerable<ProductDTO>> Search([FromQuery] ProductSearchCreteria searchCreteria)
        {
            using Stream stream = FileProvider.GetFileInfo("productData.json").CreateReadStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var reader = new Utf8JsonReader(new ReadOnlySequence<byte>(buffer));
            JsonDocument json;
            if (!JsonDocument.TryParseValue(ref reader, out json))
            {
                return this.InternalServerError();
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

            //return Ok(products.Select(x => new ProductDTO
            //{
            //    Id = x.pid,
            //    Name = x._name,
            //    Category = x.category,
            //    Quantity = x._quantity,
            //    Price = x._price,
            //    Weight = x._weight,
            //    Rating = x.rating,
            //}));
        }
    }
}
