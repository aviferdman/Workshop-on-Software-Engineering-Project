using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;
using TradingSystem.WebApi.DTO.Store;
using TradingSystem.WebApi.DTO.Store.Bids;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/stores/bids/[action]")]
    public class StoreBidsController : ControllerBase
    {
        public StoreBidsController(MarketBidsService marketBidsService)
        {
            MarketBidsService = marketBidsService;
        }

        public MarketBidsService MarketBidsService { get; }

        [HttpPost]
        public ActionResult<IEnumerable<BidStoreDTO>> Mine([FromBody] UsernameDTO usernameDTO)
        {
            if (string.IsNullOrWhiteSpace(usernameDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            Result<ICollection<Bid>>? result = MarketBidsService.GetCustomerBids(usernameDTO.Username);
            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                return InternalServerError(result.Mess);
            }

            return Ok(result.Ret.Select(BidUserDTO.FromBid));
        }

        [HttpPost]
        public ActionResult<IEnumerable<BidStoreDTO>> OfStore([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            if (string.IsNullOrWhiteSpace(storeInfoActionDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (storeInfoActionDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            Result<ICollection<Bid>>? storeBids = MarketBidsService.GetStoreBids(storeInfoActionDTO.StoreId, storeInfoActionDTO.Username);
            if (storeBids == null || (storeBids.IsErr && string.IsNullOrWhiteSpace(storeBids.Mess)))
            {
                return InternalServerError();
            }
            if (storeBids.IsErr)
            {
                return InternalServerError(storeBids.Mess);
            }

            Result<ICollection<Bid>>? myApprovedBids = MarketBidsService.GetOwnerAcceptedBids(storeInfoActionDTO.StoreId, storeInfoActionDTO.Username);
            if (myApprovedBids == null || (myApprovedBids.IsErr && string.IsNullOrWhiteSpace(myApprovedBids.Mess)))
            {
                return InternalServerError();
            }
            if (myApprovedBids.IsErr)
            {
                return InternalServerError(myApprovedBids.Mess);
            }

            IEnumerable<BidStoreDTO> result = storeBids.Ret.GroupJoin
            (
                myApprovedBids.Ret,
                bid => bid.Id,
                bid => bid.Id,
                (bid, approvedBid) => new BidStoreDTO
                {
                    Id = bid.Id,
                    ProductId = bid.ProductId,
                    Username = bid.Username,
                    Price = bid.Price,
                    Status = (DTO.Store.Bids.BidStatus)bid.Status,
                    ApprovedByMe = approvedBid.Any(),
                }
            );
            return Ok(result);
        }

        public async Task<ActionResult> ChangeBidPolicy([FromBody] BidPolicyChangeDTO bidPolicyChangeDTO)
        {
            if (string.IsNullOrWhiteSpace(bidPolicyChangeDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (bidPolicyChangeDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            Result<bool>? result = await MarketBidsService.OwnerChangeBidPolicy(bidPolicyChangeDTO.Username, bidPolicyChangeDTO.StoreId, bidPolicyChangeDTO.IsAvailable);
            if (result == null || ((result.IsErr || !result.Ret) && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr || !result.Ret)
            {
                return InternalServerError(result.Mess);
            }

            return Ok();
        }

        public async Task<ActionResult<Guid>> CreateCustomerBid([FromBody] CreateBidOfferDTO bidOfferDTO)
        {
            if (string.IsNullOrWhiteSpace(bidOfferDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (bidOfferDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (bidOfferDTO.ProductId == Guid.Empty)
            {
                return BadRequest("Invalid product ID");
            }
            if (bidOfferDTO.NewPrice < 0)
            {
                return BadRequest("Invalid price");
            }

            Result<Guid>? result = await MarketBidsService.CustomerCreateBid(bidOfferDTO.Username, bidOfferDTO.StoreId, bidOfferDTO.ProductId, bidOfferDTO.NewPrice);
            if (result == null || (result.IsErr && string.IsNullOrWhiteSpace(result.Mess)))
            {
                return InternalServerError();
            }
            if (result.IsErr)
            {
                return InternalServerError(result.Mess);
            }

            return result.Ret;
        }
    }
}
