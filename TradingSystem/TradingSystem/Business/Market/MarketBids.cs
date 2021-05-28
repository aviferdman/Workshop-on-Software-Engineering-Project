using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public class MarketBids
    {
        private MarketStores marketStores;

        private static readonly Lazy<MarketBids>
        _lazy =
        new Lazy<MarketBids>
            (() => new MarketBids());

        public static MarketBids Instance { get { return _lazy.Value; } }

        private MarketBids()
        {
            marketStores = MarketStores.Instance;
        }

        // Customer
        public async Task<Result<Guid>> CustomerCreateBid(string username, Guid storeId, Guid productId, double newBidPrice)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<Guid>(new Guid(), true, "Store doesn't exist");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<Guid>(new Guid(), true, "Bids are not supported in this store");
            }
            var bid = new Bid(username, storeId, productId, newBidPrice);
            await store.AddBid(bid);
            store.NotifyOwners(EventType.RequestPurchaseEvent, $"You got new Bid in store {store.name}");
            return new Result<Guid>(bid.Id, false, "");
        }
        public async Task<Result<bool>> CustomerNegotiateBid(Guid storeId, Guid bidId, double newBidPrice)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<bool>(false, true, "Bids are not supported in this store");
            }
            var appointer = store.founder;
            var bid = appointer.GetBidById(bidId);
            var username = bid.Username;
            var storeName = store.name;
            var productName = store.GetProduct(bid.ProductId).Name;
            store.NotifyOwners(EventType.RequestPurchaseEvent, $"{username} Suggested to buy product {productName} for {newBidPrice} in store {storeName}");
            return await appointer.CustomerNegotiateBid(bidId, newBidPrice);
        }

        public async Task<Result<bool>> CustomerDenyBid(Guid storeId, Guid bidId)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<bool>(false, true, "Bids are not supported in this store");
            }
            await store.CustomerDenyBid(bidId);
            var bid = store.GetFounder().GetBidById(bidId);
            store.NotifyOwners(EventType.RequestPurchaseEvent, $"{bid.Username} denied the bid in store {store.name} for buying the product {bid.ProductId} for {bid.Price}$");
            return new Result<bool>(true, false, "");
        }

        public Result<ICollection<Bid>> GetCustomerBids(string username)
        {
            ICollection<Bid> bids = new HashSet<Bid>();
            var stores = marketStores.LoadedStores;
            foreach (var store in stores)
            {
                ICollection<Bid> tempBids = store.Value.GetFounder().GetUserBids(username);
                foreach (var bid in tempBids)
                {
                    bids.Add(bid);
                }
            }
            return new Result<ICollection<Bid>>(bids, false, "");
        }

        // Owner
        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<bool>(false, true, "Bids are not supported in this store");
            }
            var appointer = store.GetAppointer(ownerUsername);
            var bid = appointer.GetBidById(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"We accepted your bid request.");
            return await appointer.OwnerAcceptBid(bidId);
        }

        public async Task<Result<bool>> OwnerChangeBidPolicy(string ownerUsername, Guid storeId, bool isAvailable)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            return await store.ChangeBidPolicy(isAvailable);
        }

        public async Task<Result<bool>> OwnerNegotiateBid(string ownerUsername, Guid storeId, Guid bidId, double newBidPrice)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<bool>(false, true, "Bids are not supported in this store");
            }
            var appointer = store.GetAppointer(ownerUsername);
            var bid = appointer.GetBidById(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, We suggest another bid request for you.");
            return await appointer.OwnerNegotiateBid(bidId, newBidPrice);
        }

        public async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            if (!store.CheckPermission(ownerUsername, Permission.BidRequests))
            {
                return new Result<bool>(false, true, "No permission to accept Bid");
            }
            if (!store.IsPurchaseKindAvailable(PurchaseKind.Bid))
            {
                return new Result<bool>(false, true, "Bids are not supported in this store");
            }
            var appointer = store.GetAppointer(ownerUsername);
            var bid = appointer.GetBidById(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, sorry, but we denied your bid request.");
            return await appointer.OwnerDenyBid(bidId);
        }

        public Result<ICollection<Bid>> GetOwnerBids(string ownerUsername)
        {
            ICollection<Bid> bids = new HashSet<Bid>();
            var stores = marketStores.LoadedStores;
            foreach (var store in stores)
            {
                if (store.Value.CheckPermission(ownerUsername, Permission.BidRequests))
                {
                    ICollection<Bid> tempBids = store.Value.GetFounder().Bids;
                    foreach (var bid in tempBids)
                    {
                        bids.Add(bid);
                    }
                }
            }
            return new Result<ICollection<Bid>>(bids, false, "");
        }
    }
}
