using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Service
{
    class MarketBidsService
    {
        private static readonly Lazy<MarketBidsService> instanceLazy = new Lazy<MarketBidsService>(() => new MarketBidsService(), true);

        private MarketBidsService()
        {

        }

        public static MarketBidsService Instance => instanceLazy.Value;

        // Customers 
        public async Task<Result<Guid>> CustomerCreateBid(String username, Guid storeId, Guid productId, double newBidPrice)
        {
            return await MarketBids.Instance.CustomerCreateBid(username, storeId, productId, newBidPrice);
        }
        public async Task<Result<bool>> CustomerNegotiateBid(Guid storeId, Guid bidId, double newBidPrice)
        {
            return await MarketBids.Instance.CustomerNegotiateBid(storeId, bidId, newBidPrice);
        }
        public async Task<Result<bool>> CustomerDenyBid(Guid storeId, Guid bidId)
        {
            return await MarketBids.Instance.CustomerDenyBid(storeId, bidId);
        }
        public Result<ICollection<Bid>> GetCustomerBids(string username)
        {
            return MarketBids.Instance.GetCustomerBids(username);
        }

        // Owners
        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            return await MarketBids.Instance.OwnerAcceptBid(ownerUsername, storeId, bidId);
        }
        public async Task<Result<bool>> OwnerNegotiateBid(string ownerUsername, Guid storeId, Guid bidId, double newBidPrice)
        {
            return await MarketBids.Instance.OwnerNegotiateBid(ownerUsername, storeId, bidId, newBidPrice);
        }
        public async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            return await MarketBids.Instance.OwnerDenyBid(ownerUsername, storeId, bidId);
        }
        public Result<ICollection<Bid>> GetOwnerBids(string ownerUsername)
        {
            return MarketBids.Instance.GetOwnerBids(ownerUsername);
        }
    }
}
