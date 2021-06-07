using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StorePackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Business.Market.BidsPackage
{
    public class BidsManager
    {
        public Guid id { get; set; }
        public Store s { get; set; }
        public ICollection<Bid> bids { get; set; }
        public BidsManager(Store s)
        {
            this.s = s;
            this.bids = new HashSet<Bid>();
        }

        public BidsManager()
        {
        }

        public async Task AddBid(Bid bid)
        {
            this.bids.Add(bid);
        }

        public ICollection<Bid> GetUserBids(string username)
        {
            return bids.Where(b => b.Username.Equals(username)).ToHashSet();
        }

        public ICollection<Bid> GetUserAcceptedBids(string username)
        {
            return bids.Where(b => b.Username.Equals(username) && b.Status.Equals(BidStatus.Accept)).ToHashSet();
        }

        public Bid GetBidById(Guid id)
        {
            return bids.Where(b => b.Id.Equals(id)).FirstOrDefault();
        }

        public async Task<Result<bool>> CustomerDenyBid(Guid bidId, string storeName)
        {
            var bid = GetBidById(bidId);
            NotifyOwners(EventType.RequestPurchaseEvent, $"{bid.Username} denied the bid in store {storeName} for buying the product {bid.ProductId} for {bid.Price}$");
            bid.Status = BidStatus.Deny;
            return new Result<bool>(true, false, "");
        }

        public async Task<Result<Guid>> CustomerCreateBid(string username, Guid storeId, Guid productId, double newBidPrice, string storeName)
        {
            var bid = new Bid(username, storeId, productId, newBidPrice);
            this.bids.Add(bid);
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
            bid.Status = BidStatus.Negotiate;
            return new Result<bool>(true, false, "");
        }

        public Guid getProductIdByBidId(Guid bidId)
        {
            return bids.Where(b => b.Id.Equals(bidId)).Select(b => b.ProductId).FirstOrDefault();
        }

        public async Task<Result<bool>> OwnerAcceptBid(string ownerUsername, Guid bidId)
        {
            var bid = GetBidById(bidId);
            var owner = GetAppointer(ownerUsername);
            await owner.AcceptBid(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"We accepted your bid request.");
            if (AllOwnersAccept(bidId))
            {
                bid.Status = BidStatus.Accept;
            }
            return new Result<bool>(true, false, "");
        }

        private bool AllOwnersAccept(Guid bidId)
        {
            var bidAcceptence = s.founder.GetBidAcceptenceByBidId(bidId);
            bool allAccept = bidAcceptence != null && bidAcceptence.accept;
            foreach (var owner in s.owners)
            {
                bidAcceptence = owner.GetBidAcceptenceByBidId(bidId);
                allAccept = allAccept && bidAcceptence != null && bidAcceptence.accept;
            }
            return allAccept;
        }

        public async Task<Result<bool>> OwnerNegotiateBid(string ownerUsername, Guid bidId, double newBidPrice)
        {
            var bid = GetBidById(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, We suggest another bid request for you.");
            bid.Price = newBidPrice;
            bid.Status = BidStatus.Negotiate;
            return new Result<bool>(true, false, "");
        }

        internal async Task<Result<bool>> OwnerDenyBid(string ownerUsername, Guid bidId)
        {
            var bid = GetBidById(bidId);
            var owner = GetAppointer(ownerUsername);
            await owner.DenyBid(bidId);
            PublisherManagement.Instance.EventNotification(bid.Username, EventType.RequestPurchaseEvent, $"Hi {bid.Username}, sorry, but we denied your bid request.");
            bid.Status = BidStatus.Deny;
            return new Result<bool>(true, false, "");
        }

        private Owner GetOwner(String name)
        {
            foreach (Owner owner in s.owners)
            {
                if (owner.Username.Equals(name))
                    return owner;
            }
            return null;
        }
        private Appointer GetAppointer(string appointerName)
        {
            Appointer ret = GetOwner(appointerName);
            if (ret == null)
            {
                ret = s.founder;
            }
            return ret;
        }
    }
}
