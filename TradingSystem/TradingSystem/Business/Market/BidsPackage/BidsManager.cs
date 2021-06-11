using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Business.Market.BidsPackage
{
    public class BidsManager
    {
        public Guid id { get; set; }
        public Store s { get; set; }
        public HashSet<BidState> bidsState { get; set; }
        public BidsManager(Store s)
        {
            this.s = s;
            this.bidsState = new HashSet<BidState>();
        }

        public BidsManager()
        {
        }

        public ICollection<Bid> GetUserBids(string username)
        {
            return bidsState.Select(state => state.Bid).Where(b => b.Username.Equals(username)).ToHashSet();
        }

        public ICollection<Bid> GetUserAcceptedBids(string username)
        {
            return bidsState.Select(state => state.Bid).Where(b => b.Username.Equals(username) && b.Status.Equals(BidStatus.Accept)).ToHashSet();
        }

        public Bid GetBidById(Guid id)
        {
            return bidsState.Select(state => state.Bid).Where(b => b.Id.Equals(id)).FirstOrDefault();
        }

        public BidState GetBidStateById(Guid id)
        {
            return bidsState.Where(state => state.Bid.Id.Equals(id)).FirstOrDefault();
        }

        public async Task<Result<bool>> CustomerDenyBid(Guid bidId, string storeName)
        {
            var bid = GetBidById(bidId);
            NotifyOwners(EventType.RequestPurchaseEvent, $"{bid.Username} denied the bid in store {storeName} for buying the product {bid.ProductId} for {bid.Price}$");
            bid.Status = BidStatus.Deny;
            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }

        public async Task<Result<Guid>> CustomerCreateBid(string username, Guid storeId, Guid productId, double newBidPrice, string storeName)
        {
            Bid bid = new Bid(username, storeId, productId, newBidPrice);
            BidState b = new BidState(bid);
            if (!ProxyMarketContext.Instance.IsDebug)
            {
                MarketContext.Instance.Bids.Add(bid);
                await ProxyMarketContext.Instance.saveChanges();
                MarketContext.Instance.BidStates.Add(b);
                await ProxyMarketContext.Instance.saveChanges();
            }
           
            this.bidsState.Add(b);
           
            await ProxyMarketContext.Instance.saveChanges();
            NotifyOwners(EventType.RequestPurchaseEvent, $"You got new Bid in store {storeName}");
            return new Result<Guid>(bid.Id, false, "");
        }

        public void NotifyOwners(EventType eventType, string message)
        {
            //notify the founder for a new purchase
            PublisherManagement.Instance.EventNotification(s.founder.Username, eventType, message);

            //notify the owners
            foreach (var owner in s.owners)
            {
                PublisherManagement.Instance.EventNotification(owner.Username, eventType, message);
            }
        }

        internal async Task<Result<bool>> CustomerNegotiateBid(Guid bidId, double newBidPrice, string storeName, string productName)
        {
            var bid = GetBidById(bidId);
            var username = bid.Username;
            NotifyOwners(EventType.RequestPurchaseEvent, $"{username} Suggested to buy product {productName} for {newBidPrice} in store {storeName}");
            bid.Price = newBidPrice;
            bid.Status = BidStatus.CustomerNegotiate;
            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }

        internal async Task<Result<bool>> CustomerAcceptBid(Guid bidId, string storeName, string productName)
        {
            var bid = GetBidById(bidId);
            if (bid.Status.Equals(BidStatus.CustomerNegotiate))
            {
                return new Result<bool>(false, true, "You cant accept your own suggestion.");
            }
            var username = bid.Username;
            NotifyOwners(EventType.RequestPurchaseEvent, $"{username} Accepted to buy product {productName} for {bid.Price} in store {storeName}");
            bid.Status = BidStatus.Accept;
            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }

        public Guid getProductIdByBidId(Guid bidId)
        {
            return bidsState.Select(state => state.Bid).Where(b => b.Id.Equals(bidId)).Select(b => b.ProductId).FirstOrDefault();
        }

        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid bidId)
        {
            var bid = GetBidById(bidId);
            if (bid.Status.Equals(BidStatus.OwnerNegotiate))
            {
                return new Result<bool>(false, true, "You cant accept your own suggestion.");
            }
            var bidState = GetBidStateById(bidId);
            await bidState.AddAcceptence(ownerUsername);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"We accepted your bid request.");
            if (AllOwnersAccept(bidId))
            {
                bid.Status = BidStatus.Accept;
            }

            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }

        private bool AllOwnersAccept(Guid bidId)
        {
            var bidState = GetBidStateById(bidId);
            bool allAccept = bidState.OwnersAccepted.Where(p=>p.p.Equals(s.founder.Username)).Any();
            foreach (var owner in s.owners)
            {
                allAccept = allAccept && bidState.OwnersAccepted.Where(p => p.p.Equals(owner.Username)).Any();
            }
            return allAccept;
        }

        public async Task<Result<bool>> OwnerNegotiateBid(string ownerUsername, Guid bidId, double newBidPrice)
        {
            var bid = GetBidById(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, We suggest another bid request for you.");
            bid.Price = newBidPrice;
            bid.Status = BidStatus.OwnerNegotiate;
            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }

        internal async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid bidId)
        {
            var bid = GetBidById(bidId);
            var bidState = GetBidStateById(bidId);
            await bidState.RemoveAcceptence(ownerUsername);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, sorry, but we denied your bid request.");
            bid.Status = BidStatus.Deny;
            await ProxyMarketContext.Instance.saveChanges();
            return new Result<bool>(true, false, "");
        }
    }
}
