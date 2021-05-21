using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.UserPackage;

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

        public async Task removeManager(Manager manager)
        {
            await proxyMarketContext.removeManager(manager);
        }

        public void removeProductFromCart(ProductInCart productInCart)
        {
            proxyMarketContext.removeProductFromCart(productInCart);
        }

        public async Task AddStore(Store store)
        {
            await proxyMarketContext.AddStore(store);
        }

        public async Task<Store> getStore(Guid storeId)
        {
            return await proxyMarketContext.getStore(storeId);
        }

        public async Task removeOwner(Owner ownerToRemove)
        {
            await proxyMarketContext.removeOwner(ownerToRemove);
        }

        public async Task<ICollection<Store>> GetStoresByName(string name)
        {
            return await proxyMarketContext.GetStoresByName( name);
        }

        public void findStoreProduct(out Store found, out Product p, Guid pid)
        {
           proxyMarketContext.findStoreProduct(out  found, out  p,  pid);
        }

        public async Task<Category> AddNewCategory(string category)
        {
            return await proxyMarketContext.AddNewCategory(category);
        }

        public async Task<ICollection<Product>> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            return await proxyMarketContext.findProducts( keyword,  price_range_low,  price_range_high,  rating,  category);
        }
    }
}
