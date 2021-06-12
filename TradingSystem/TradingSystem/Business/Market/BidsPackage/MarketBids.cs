using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.DAL;
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
            return await store.CustomerCreateBid(username, storeId, productId, newBidPrice);
        }

        public async Task<Result<bool>> CustomerAcceptBid(Guid storeId, Guid bidId)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }

            return await store.CustomerAcceptBid(bidId);
        }

        public async Task<Result<bool>> CustomerNegotiateBid(Guid storeId, Guid bidId, double newBidPrice)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }

            return await store.CustomerNegotiateBid(bidId, newBidPrice);
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
            return await store.CustomerDenyBid(bidId);
        }

        public Result<ICollection<Bid>> GetCustomerBids(string username)
        {
            ICollection<Bid> bids = ProxyMarketContext.Instance.getUserBids(username);
           
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

            return await store.OwnerAcceptBid(ownerUsername, bidId);
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
            return await store.OwnerNegotiateBid(ownerUsername, bidId, newBidPrice);
        }

        public async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            Store store = await marketStores.GetStoreById(storeId);
            if (store == null)
            {
                return new Result<bool>(false, true, "Store doesn't exist");
            }
            return await store.OwnerDenyBid(ownerUsername, bidId);
        }

        public Result<ICollection<Bid>> GetStoreBids(Guid storeId, string ownerUsername)
        {
            var stores = marketStores.LoadedStores;
            var id_store = stores.Where(s => s.Key.Equals(storeId)).FirstOrDefault();
            if (id_store.Equals(default(KeyValuePair<Guid, Store>)))
            {
                return new Result<ICollection<Bid>>(new HashSet<Bid>(), true, "No such store.");
            }
            var store = id_store.Value;
            return store.GetBids(ownerUsername);
        }

        public Result<ICollection<Bid>> GetOwnerAcceptedBids(Guid storeId, string ownerUsername)
        {
            var stores = marketStores.LoadedStores;
            var id_store = stores.Where(s => s.Key.Equals(storeId)).FirstOrDefault();
            if (id_store.Equals(default(KeyValuePair<Guid, Store>)))
            {
                return new Result<ICollection<Bid>>(new HashSet<Bid>(), true, "No such store.");
            }
            var store = id_store.Value;
            return store.GetOwnerAcceptedBids(ownerUsername);
        }
    }
}
