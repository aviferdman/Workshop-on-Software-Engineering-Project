using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.PolicyPackage;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.Store;
using TradingSystem.WebApi.DTO.Store.Discounts;
using TradingSystem.WebApi.DTO.Store.Policy;

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
        public async Task<ActionResult<Guid>> AddDiscount([FromBody] DiscountParamsSimpleCreationDTO discountParamsSimpleCreationDTO)
        {
            if (string.IsNullOrWhiteSpace(discountParamsSimpleCreationDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (discountParamsSimpleCreationDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }

            DiscountParamsSimpleDTO discountParamsDTO = discountParamsSimpleCreationDTO.Params;

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
                    discountParamsSimpleCreationDTO.Username,
                    discountParamsSimpleCreationDTO.StoreId,
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
                    discountParamsSimpleCreationDTO.Username,
                    discountParamsSimpleCreationDTO.StoreId,
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
        public async Task<ActionResult<Guid>> EditDiscount([FromBody] DiscountParamsSimpleEditDTO discountParamsSimpleEditDTO)
        {
            if (string.IsNullOrWhiteSpace(discountParamsSimpleEditDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (discountParamsSimpleEditDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }
            if (discountParamsSimpleEditDTO.DiscountId == Guid.Empty)
            {
                return BadRequest("Invalid discount id");
            }

            DiscountParamsSimpleDTO discountParamsDTO = discountParamsSimpleEditDTO.Params;

            RuleContext ruleContext;
            if (!Enum.TryParse(discountParamsDTO.DiscountType, out ruleContext))
            {
                return BadRequest("Invalid discount type");
            }

            Task<Result<Guid>> task;
            if (string.IsNullOrEmpty(discountParamsDTO.ConditionType))
            {
                task = MarketRulesService.UpdateSimpleDiscountAsync
                (
                    discountParamsSimpleEditDTO.DiscountId,
                    discountParamsSimpleEditDTO.Username,
                    discountParamsSimpleEditDTO.StoreId,
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

                task = MarketRulesService.UpdateConditionalDiscountAsync
                (
                    discountParamsSimpleEditDTO.DiscountId,
                    discountParamsSimpleEditDTO.Username,
                    discountParamsSimpleEditDTO.StoreId,
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

            Result<Guid> result = await task;
            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                return InternalServerError(result.Mess);
            }

            Guid id = result.Ret;
            if (id == Guid.Empty)
            {
                return InternalServerError();
            }

            return Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddCompoundDiscount([FromBody] DiscountCompoundParamsDTO discountCompoundParamsDTO)
        {
            DiscountRuleRelation discountRuleRelation;
            if (string.IsNullOrWhiteSpace(discountCompoundParamsDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (!Enum.TryParse(discountCompoundParamsDTO.DiscountRuleRelation, out discountRuleRelation))
            {
                return BadRequest("Invalid rule relation");
            }
            if (discountCompoundParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }
            if (discountCompoundParamsDTO.DiscountId1 == Guid.Empty || discountCompoundParamsDTO.DiscountId2 == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }

            Guid result = await MarketRulesService.AddDiscountRuleAsync
            (
                discountCompoundParamsDTO.Username,
                discountRuleRelation,
                discountCompoundParamsDTO.StoreId,
                discountCompoundParamsDTO.DiscountId1,
                discountCompoundParamsDTO.DiscountId2,
                discountCompoundParamsDTO.Decide
            );
            if (result == Guid.Empty)
            {
                return InternalServerError();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> RemoveDiscount([FromBody] DiscountRefActionDTO discountRefActionDTO)
        {
            if (string.IsNullOrWhiteSpace(discountRefActionDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (discountRefActionDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }
            if (discountRefActionDTO.DiscountId == Guid.Empty)
            {
                return BadRequest("Invalid discount id");
            }

            Result<Guid>? result = await MarketRulesService.RemoveDiscountAsync
            (
                discountRefActionDTO.Username,
                discountRefActionDTO.StoreId,
                discountRefActionDTO.DiscountId
            );
            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                return InternalServerError(result.Mess);
            }

            Guid id = result.Ret;

            return Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult<AllDiscountsDTO>> Discounts([FromBody] GuidDTO guidDTO)
        {
            if (guidDTO.Id == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            ICollection<DiscountData>? leafDiscounts = await MarketRulesService.GetAllDiscounts(guidDTO.Id);
            if (leafDiscounts == null)
            {
                return InternalServerError();
            }

            ICollection<DiscountsRelation>? discountsRelations = await MarketRulesService.GetAllDiscountsRelations(guidDTO.Id);
            if (discountsRelations == null)
            {
                return InternalServerError();
            }

            return Ok(new AllDiscountsDTO
            {
                 LeafDiscounts = leafDiscounts.Select(DiscountDataDTO.FromDiscountData),
                 RelationDiscounts = discountsRelations.Select(DiscountDataCompundDTO.FromDiscountRelation),
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddPolicy([FromBody] PolicyParamsDTO policyParamsDTO)
        {
            RuleContext ruleContext;
            if (!Enum.TryParse(policyParamsDTO.RuleContext, out ruleContext))
            {
                return BadRequest("Invalid rule context");
            }

            RuleType ruleType;
            if (!Enum.TryParse(policyParamsDTO.RuleType, out ruleType))
            {
                return BadRequest("Invalid rule type");
            }

            PolicyRuleRelation policyRuleRelation;
            if (!Enum.TryParse(policyParamsDTO.RuleRelation, out policyRuleRelation))
            {
                return BadRequest("Invalid rule relation");
            }

            await MarketRulesService.AddPolicyRule
            (
                policyParamsDTO.Username,
                policyParamsDTO.StoreId,
                policyRuleRelation,
                ruleContext,
                ruleType,
                policyParamsDTO.Category,
                policyParamsDTO.ProductId ?? Guid.Empty,
                policyParamsDTO.MaxValue ?? int.MaxValue,
                policyParamsDTO.MinValue ?? 0,
                policyParamsDTO.StartDate ?? default,
                policyParamsDTO.EndDate ?? default
            );

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> RemovePolicies([FromBody] StoreInfoActionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                return BadRequest("Invalid username");
            }
            if (dto.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store id");
            }

            await MarketRulesService.RemovePolicyRule(dto.Username, dto.StoreId);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<PolicyDataDTO>>> Policies([FromBody] GuidDTO guidDTO)
        {
            if (guidDTO.Id == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            List<PolicyData>? policies = await MarketRulesService.GetAllPolicies(guidDTO.Id);
            if (policies == null)
            {
                return InternalServerError();
            }

            return Ok(policies.Select(PolicyDataDTO.FromPolicyData));
        }
    }
}
