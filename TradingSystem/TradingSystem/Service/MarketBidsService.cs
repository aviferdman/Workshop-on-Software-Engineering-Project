using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        static SemaphoreSlim semaphoreSlim;

        private MarketBidsService()
        {
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public static MarketBidsService Instance => instanceLazy.Value;

        // Customers 
        public async Task<Result<Guid>> CustomerCreateBid(String username, Guid storeId, Guid productId, double newBidPrice)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.CustomerCreateBid(username, storeId, productId, newBidPrice);
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }
        public async Task<Result<bool>> CustomerNegotiateBid(Guid storeId, Guid bidId, double newBidPrice)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.CustomerNegotiateBid(storeId, bidId, newBidPrice);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task<Result<bool>> CustomerDenyBid(Guid storeId, Guid bidId)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.CustomerDenyBid(storeId, bidId);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public Result<ICollection<Bid>> GetCustomerBids(string username)
        {
            return MarketBids.Instance.GetCustomerBids(username);
        }

        // Owners
        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.OwnerAcceptBid(ownerUsername, storeId, bidId);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task<Result<bool>> OwnerNegotiateBid(string ownerUsername, Guid storeId, Guid bidId, double newBidPrice)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.OwnerNegotiateBid(ownerUsername, storeId, bidId, newBidPrice);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.OwnerDenyBid(ownerUsername, storeId, bidId);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task<Result<bool>> OwnerChangeBidPolicy(string ownerUsername, Guid storeId, bool isAvailable)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await MarketBids.Instance.OwnerChangeBidPolicy(ownerUsername, storeId, isAvailable);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public Result<ICollection<Bid>> GetOwnerBids(string ownerUsername)
        {
            return MarketBids.Instance.GetOwnerBids(ownerUsername);
        }
    }
}
