using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.Store.Discounts;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/stores/rules/[action]")]
    public class StoreRulesController : ControllerBase
    {
        public StoreRulesController() : this(MarketRulesService.Instance) { }
        private StoreRulesController(MarketRulesService marketRulesService)
        {
            MarketRulesService = marketRulesService;
        }

        public MarketRulesService MarketRulesService { get; }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddSimpleDiscount([FromBody] SimpleDiscountParamsDTO simpleDiscountParamsDTO)
        {
            RuleContext ruleContext;
            if (!Enum.TryParse(simpleDiscountParamsDTO.DiscountType, out ruleContext))
            {
                return BadRequest("Invalid discount type");
            }

            Guid result = await MarketRulesService.AddSimpleDiscountAsync
            (
                simpleDiscountParamsDTO.Username,
                simpleDiscountParamsDTO.StoreId,
                ruleContext,
                simpleDiscountParamsDTO.Percent,
                simpleDiscountParamsDTO.Category,
                simpleDiscountParamsDTO.ProductId ?? Guid.Empty
            );
            if (result == Guid.Empty)
            {
                return InternalServerError();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DiscountData>>> Discounts([FromBody] GuidDTO guidDTO)
        {
            if (guidDTO.Id == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            ICollection<DiscountData>? result = await MarketRulesService.GetAllDiscounts(guidDTO.Id);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(result.Select(DiscountDataDTO.FromDiscountData));
        }
    }
}
