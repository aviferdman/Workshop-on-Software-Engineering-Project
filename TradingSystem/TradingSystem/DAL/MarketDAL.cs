using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

namespace TradingSystem.DAL
{
    class MarketDAL
    {
        ProxyMarketContext proxyMarketContext = ProxyMarketContext.Instance;
        public static MarketDAL Instance { get { return _lazy.Value; } }

        private static readonly Lazy<MarketDAL>
       _lazy =
       new Lazy<MarketDAL>
           (() => new MarketDAL());

        public async Task<MemberState> getMemberState(string usrname)
        {
            return await proxyMarketContext.getMemberState(usrname);
        }

        public async Task<ShoppingCart> getShoppingCart(string usrname)
        {
            return await proxyMarketContext.getShoppingCart(usrname);
        }

        public async Task AddHistory(TransactionStatus history)
        {
            await proxyMarketContext.AddHistory(history);
        }

        public async Task<ICollection<TransactionStatus>> getAllHistories()
        {
            return await proxyMarketContext.getAllHistories();
        }

        public async Task<ICollection<TransactionStatus>> getUserHistories(string username)
        {
            return await proxyMarketContext.getUserHistories(username);
        }

        public async Task<ICollection<TransactionStatus>> getStoreHistories(Guid storeid)
        {
            return await proxyMarketContext.getStoreHistories(storeid);
        }

        public void teardown()
        {
            proxyMarketContext.marketTearDown();
        }

        public async Task<ICollection<Store>> getMemberStores(string usrname)
        {
            return await proxyMarketContext.getMemberStores(usrname);
        }

        public async Task RemoveProduct(Product p)
        {
             await proxyMarketContext.RemoveProduct(p);
        }

        }
}
