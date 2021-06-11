using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market
{
    public enum PurchaseKind
    {
        Bid
    }
    public class PurchasePolicy
    {
        public Guid id { get; set; }
        public HashSet<Prem> availablePurchaseKinds { get; set; }
        public PurchasePolicy()
        {
            availablePurchaseKinds = new HashSet<Prem>();
        }
        public async Task AddPurchaseKind(PurchaseKind purchaseKind, ProxyMarketContext p)
        {
            var toAdd = purchaseKind.ToString();
            if (!Contains(toAdd))
            {
                availablePurchaseKinds.Add(new Prem(toAdd));
                await p.saveChanges();
            };
        }

        public async Task RemovePurchaseKind(PurchaseKind purchaseKind)
        {
            var toRemove = purchaseKind.ToString();
            if (Contains(toRemove))
            {
                availablePurchaseKinds.Remove(new Prem(toRemove));
            }
        }

        public bool IsAvalable(PurchaseKind purchaseKind)
        {
            return Contains(purchaseKind.ToString());
        }

        public bool Contains(string s)
        {
            return this.availablePurchaseKinds.Count(P => P.p.Equals(s)) > 0;
        }
    }
}