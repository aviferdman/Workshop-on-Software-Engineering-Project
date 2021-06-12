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
        public StoreBidsController(MarketBidsService marketBidsService, MarketStoreGeneralService marketStoreGeneralService)
        {
            MarketBidsService = marketBidsService;
            MarketStoreGeneralService = marketStoreGeneralService;
        }

        public MarketBidsService MarketBidsService { get; }
        public MarketStoreGeneralService MarketStoreGeneralService { get; }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<BidUserDTO>>> MineAsync([FromBody] UsernameDTO usernameDTO)
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

            // TODO: fetch store names as well using a designated service layer function
            IEnumerable<BidUserDTO> userBids = result.Ret.Select(BidUserDTO.FromBid).ToList();
            foreach (BidUserDTO userBid in userBids)
            {
                StoreData? store = await MarketStoreGeneralService.getStoreById(userBid.StoreId);
                if (store == null)
                {
                    return InternalServerError();
                }

                userBid.StoreName = store.Name;
                ProductData? product = store.Products.FirstOrDefault(p => p.pid == userBid.ProductId);
                userBid.ProductName = product?._name;
            }
            return Ok(userBids);
        }

        private ActionResult OfStoreCore(Guid storeId, string? username, Func<Bid, bool>? filter, out IEnumerable<BidStoreDTO>? bids)
        {
            bids = null;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Invalid username");
            }
            if (storeId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }

            Result<ICollection<Bid>>? storeBidsResult = MarketBidsService.GetStoreBids(storeId, username);
            if (storeBidsResult == null || (storeBidsResult.IsErr && string.IsNullOrWhiteSpace(storeBidsResult.Mess)))
            {
                return InternalServerError();
            }
            if (storeBidsResult.IsErr)
            {
                return InternalServerError(storeBidsResult.Mess);
            }

            IEnumerable<Bid> storeBids = storeBidsResult.Ret;
            if (filter != null)
            {
                storeBids = storeBids.Where(filter);
            }

            Result<ICollection<Bid>>? myApprovedBidsResult = MarketBidsService.GetOwnerAcceptedBids(storeId, username);
            if (myApprovedBidsResult == null || (myApprovedBidsResult.IsErr && string.IsNullOrWhiteSpace(myApprovedBidsResult.Mess)))
            {
                return InternalServerError();
            }
            if (myApprovedBidsResult.IsErr)
            {
                return InternalServerError(myApprovedBidsResult.Mess);
            }

            IEnumerable<Bid> myApprovedBids = myApprovedBidsResult.Ret;
            if (filter != null)
            {
                myApprovedBids = myApprovedBids.Where(filter);
            }

            IEnumerable<BidStoreDTO> result = storeBids.GroupJoin
            (
                myApprovedBids,
                bid => bid.Id,
                bid => bid.Id,
                (bid, approvedBid) =>
                {
                    var bidStoreDTO = BidStoreDTO.FromBid(bid);
                    bidStoreDTO.ApprovedByMe = approvedBid.Any();
                    return bidStoreDTO;
                }
            );

            bids = result;
            return null;
        }

        [HttpPost]
        public ActionResult<IEnumerable<BidStoreDTO>> OfStore([FromBody] StoreInfoActionDTO storeInfoActionDTO)
        {
            ActionResult actionResult = OfStoreCore(storeInfoActionDTO.StoreId, storeInfoActionDTO.Username, null, out IEnumerable<BidStoreDTO>? bids);
            if (actionResult != null)
            {
                return actionResult;
            }
            if (bids == null)
            {
                return InternalServerError();
            }

            return Ok(bids);
        }

        [HttpPost]
        public ActionResult<BidStoreDTO> OfStoreSpecific([FromBody] OwnerBidParamsDTO ownerBidParamsDTO)
        {
            // TODO: call a specific service layer function
            ActionResult actionResult = OfStoreCore
            (
                ownerBidParamsDTO.StoreId,
                ownerBidParamsDTO.Username,
                bid => bid.Id == ownerBidParamsDTO.BidId,
                out IEnumerable<BidStoreDTO>? bids
            );
            if (actionResult != null)
            {
                return actionResult;
            }
            if (bids == null)
            {
                return InternalServerError();
            }

            BidStoreDTO? bid = null;
            try
            {
                bid = bids.SingleOrDefault();
            }
            catch (InvalidOperationException) { }

            if (bid == null)
            {
                return InternalServerError("More than 1 bids match the specified ID");
            }

            return Ok(bid);
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

            Guid id = result.Ret;
            if (id == Guid.Empty)
            {
                return InternalServerError();
            }

            return Ok(result.Ret);
        }

        public async Task<ActionResult> CustomerAcceptBid([FromBody] CustomerBidParamsDTO customerBidParamsDTO)
        {
            if (customerBidParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (customerBidParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.CustomerAcceptBid(customerBidParamsDTO.StoreId, customerBidParamsDTO.BidId);
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

        public async Task<ActionResult> CustomerNegotiateBid([FromBody] CustomerNegotiateBidParamsDTO customerNegotiateBidParamsDTO)
        {
            if (customerNegotiateBidParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (customerNegotiateBidParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.CustomerNegotiateBid
            (
                customerNegotiateBidParamsDTO.StoreId,
                customerNegotiateBidParamsDTO.BidId,
                customerNegotiateBidParamsDTO.NewPrice
            );
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

        public async Task<ActionResult> CustomerDenyBid([FromBody] CustomerBidParamsDTO customerBidParamsDTO)
        {
            if (customerBidParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (customerBidParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.CustomerDenyBid(customerBidParamsDTO.StoreId, customerBidParamsDTO.BidId);
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

        public async Task<ActionResult> OwnerAcceptBid([FromBody] OwnerBidParamsDTO ownerBidParamsDTO)
        {
            if (string.IsNullOrWhiteSpace(ownerBidParamsDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (ownerBidParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (ownerBidParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.OwnerAcceptBid
            (
                ownerBidParamsDTO.Username,
                ownerBidParamsDTO.StoreId,
                ownerBidParamsDTO.BidId
            );
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

        public async Task<ActionResult> OwnerNegotiateBid([FromBody] OwnerNegotiateParamsDTO ownerNegotiateParamsDTO)
        {
            if (string.IsNullOrWhiteSpace(ownerNegotiateParamsDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (ownerNegotiateParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (ownerNegotiateParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.OwnerNegotiateBid
            (
                ownerNegotiateParamsDTO.Username,
                ownerNegotiateParamsDTO.StoreId,
                ownerNegotiateParamsDTO.BidId,
                ownerNegotiateParamsDTO.NewPrice
            );
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

        public async Task<ActionResult> OwnerDenyBid([FromBody] OwnerBidParamsDTO ownerBidParamsDTO)
        {
            if (string.IsNullOrWhiteSpace(ownerBidParamsDTO.Username))
            {
                return BadRequest("Invalid username");
            }
            if (ownerBidParamsDTO.StoreId == Guid.Empty)
            {
                return BadRequest("Invalid store ID");
            }
            if (ownerBidParamsDTO.BidId == Guid.Empty)
            {
                return BadRequest("Invalid bid ID");
            }

            Result<bool>? result = await MarketBidsService.OwnerDenyBid
            (
                ownerBidParamsDTO.Username,
                ownerBidParamsDTO.StoreId,
                ownerBidParamsDTO.BidId
            );
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
    }
}
