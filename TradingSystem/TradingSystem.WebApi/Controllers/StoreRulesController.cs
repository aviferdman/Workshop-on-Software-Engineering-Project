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
        public async Task<ActionResult<Guid>> AddDiscount([FromBody] DiscountParamsDTO discountParamsDTO)
        {
            RuleContext ruleContext;
            if (!Enum.TryParse(discountParamsDTO.DiscountType, out ruleContext))
            {
                return BadRequest("Invalid discount type");
            }

            Task<Guid> task;
            if (string.IsNullOrEmpty(discountParamsDTO.ConditionType))
            {
                task = MarketRulesService.AddSimpleDiscountAsync
                (
                    discountParamsDTO.Username,
                    discountParamsDTO.StoreId,
                    ruleContext,
                    discountParamsDTO.Percent,
                    discountParamsDTO.Category,
                    discountParamsDTO.ProductId ?? Guid.Empty
                );
            }
            else
            {
                RuleType ruleType;
                if (!Enum.TryParse(discountParamsDTO.ConditionType, out ruleType))
                {
                    return BadRequest("Invalid condition type");
                }

                task = MarketRulesService.AddConditionalDiscountAsync
                (
                    discountParamsDTO.Username,
                    discountParamsDTO.StoreId,
                    ruleContext,
                    ruleType,
                    discountParamsDTO.Percent,
                    discountParamsDTO.Category,
                    discountParamsDTO.ProductId ?? Guid.Empty,
                    discountParamsDTO.MaxValue ?? int.MaxValue,
                    discountParamsDTO.MinValue ?? 0,
                    discountParamsDTO.StartDate ?? default,
                    discountParamsDTO.EndDate ?? default
                );
            }

            Guid result = await task;
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
