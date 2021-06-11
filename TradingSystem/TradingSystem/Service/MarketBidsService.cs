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
    public class MarketBidsService
    {
     
        static SemaphoreSlim semaphoreSlim;
        private MarketBids marketBids;

        private MarketBidsService(MarketBids m)
        {
            semaphoreSlim = new SemaphoreSlim(1, 1);
            marketBids = m;
        }


        // Customers 
        public async Task<Result<Guid>> CustomerCreateBid(String username, Guid storeId, Guid productId, double newBidPrice)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await marketBids.CustomerCreateBid(username, storeId, productId, newBidPrice);
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
                return await marketBids.CustomerNegotiateBid(storeId, bidId, newBidPrice);
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
                return await marketBids.CustomerDenyBid(storeId, bidId);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public Result<ICollection<Bid>> GetCustomerBids(string username)
        {
            return marketBids.GetCustomerBids(username);
        }

        // Owners
        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid storeId, Guid bidId)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await marketBids.OwnerAcceptBid(ownerUsername, storeId, bidId);
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
                return await marketBids.OwnerNegotiateBid(ownerUsername, storeId, bidId, newBidPrice);
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
                return await marketBids.OwnerDenyBid(ownerUsername, storeId, bidId);
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
                return await marketBids.OwnerChangeBidPolicy(ownerUsername, storeId, isAvailable);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public Result<ICollection<Bid>> GetStoreBids(Guid storeId, string ownerUsername)
        {
            return marketBids.GetStoreBids(storeId, ownerUsername);
        }

        public Result<ICollection<Bid>> GetOwnerAcceptedBids(Guid storeId, string ownerUsername)
        {
            return marketBids.GetOwnerAcceptedBids(storeId, ownerUsername);
        }
    }
}
