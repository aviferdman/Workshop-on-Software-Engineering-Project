using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.UserManagement;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using System.Linq;

namespace TradingSystem.DAL
{
    class ProxyMarketContext
    {
        private bool isDebug;

        private ConcurrentDictionary<string, DataUser> dataUsers;
        private ConcurrentDictionary<string, RegisteredAdmin> admins;
        private ConcurrentDictionary<string, MemberState> memberStates;
        private ConcurrentDictionary<Guid, Store> stores;
        private ConcurrentDictionary<string, ShoppingCart> shoppingCarts;
        private HashSet<TransactionStatus> transactionStatuses;
        public bool IsDebug { get => isDebug; set => isDebug = value; }

        private MarketContext marketContext = MarketContext.Instance;
        public static ProxyMarketContext Instance { get { return _lazy.Value; } }

        public async Task saveChanges()
        {
            if (!isDebug)
                await marketContext.SaveChangesAsync();
        }
        public async Task AddHistory(TransactionStatus history)
        {
            if (isDebug)
            {
                transactionStatuses.Add(history);
            }
            try
            {
                await marketContext.AddHistory(history);
            }
            catch (Exception e)
            {
            }
        }

        private static readonly Lazy<ProxyMarketContext>
       _lazy =
       new Lazy<ProxyMarketContext>
           (() => new ProxyMarketContext());

        public async Task<ICollection<TransactionStatus>> getAllHistories()
        {
            if (isDebug)
            {
                return transactionStatuses;
            }
            try
            {
                return await marketContext.getAllHistories();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ICollection<TransactionStatus>> getUserHistories(string username)
        {
            if (isDebug)
            {
                return transactionStatuses.Where(t=> t.username==username).ToList();
            }
            try
            {
                return await marketContext.getUserHistories(username);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ICollection<TransactionStatus>> getStoreHistories(Guid storeId)
        {
            if (isDebug)
            {
                return transactionStatuses.Where(t => t.storeID == storeId).ToList();
            }
            try
            {
                return await marketContext.getStoreHistories(storeId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void marketTearDown()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            memberStates = new ConcurrentDictionary<string, MemberState>();
            shoppingCarts = new ConcurrentDictionary<string, ShoppingCart>();
            stores = new ConcurrentDictionary<Guid, Store>();
            transactionStatuses = new HashSet<TransactionStatus>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFUALT_ADMIN", "ADMIN", new Address("Israel", "Beer Sheva", "lala", "5", "1111111"), "0501234566");
            dataUsers.TryAdd("DEFUALT_ADMIN", admin);
            admins.TryAdd("DEFUALT_ADMIN", admin);
        }

        public async Task<ICollection<Store>> getMemberStores(string usrname)
        {
            if (isDebug)
            {
                List<Store> stors = new List<Store>();
                foreach(Store s in stores.Values)
                {
                    if (s.founder.username.Equals(usrname))
                    {
                        stors.Add(s);
                        continue;
                    }
                    foreach (Manager f in s.managers)
                    {
                        if (f.username.Equals(usrname))
                        {
                            stors.Add(s);
                            break;
                        }
                        
                    }
                    foreach (Owner f in s.owners)
                    {
                        if (f.username.Equals(usrname))
                        {
                            stors.Add(s);
                            break;
                        }
                    }
                }
                
                return stors;
            }
            try
            {
                return await marketContext.getMemberStores(usrname);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ProxyMarketContext()
        {
            isDebug = false;
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            memberStates = new ConcurrentDictionary<string, MemberState>();
            shoppingCarts = new ConcurrentDictionary<string, ShoppingCart>();
            stores = new ConcurrentDictionary<Guid, Store>();
            transactionStatuses = new HashSet<TransactionStatus>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFUALT_ADMIN", "ADMIN", new Address("Israel", "Beer Sheva", "lala", "5", "1111111"), "0501234566");
            dataUsers.TryAdd("DEFUALT_ADMIN", admin);
            admins.TryAdd("DEFUALT_ADMIN", admin);
        }

        public async Task AddNewMemberState(string username)
        {
            if (isDebug)
            {
                memberStates.TryAdd(username, new MemberState(username));
            }
            try
            {
                await marketContext.AddNewMemberState(username);
            }
            catch (Exception e)
            {
            }
        }

        public async Task AddNewShoppingCart(string username)
        {
            if (isDebug)
            {
                shoppingCarts.TryAdd(username, new ShoppingCart(username));
            }
            try
            {
                await marketContext.AddNewShoppingCart(username);
            }
            catch (Exception e)
            {
            }
        }

        internal void UserTearDown()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
        }


        public async Task<MemberState> getMemberState(string usrname)
        {
            if (isDebug)
            {
                MemberState m;
                memberStates.TryGetValue(usrname, out m);
                return m;
            }
            try
            {
                return await marketContext.getMemberState(usrname);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Store> getStore(Guid storeId)
        {
            if (isDebug)
            {
                Store s;
                stores.TryGetValue(storeId, out s);
                return s;
            }
            try
            {
                return await marketContext.getStore(storeId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ShoppingCart> getShoppingCart(string usrname)
        {
            if (isDebug)
            {
                ShoppingCart s;
                shoppingCarts.TryGetValue(usrname, out s);
                return s;
            }
            try
            {
                return await marketContext.getShoppingCart(usrname);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<DataUser> GetDataUser(string username)
        {
            if (isDebug)
            {
                DataUser u = null;
                dataUsers.TryGetValue(username, out u);
                return u;
            }
            try
            {
                return await marketContext.GetDataUser(username);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> RemoveDataUser(string username)
        {
            if (isDebug)
            {
                DataUser u = null;
                return dataUsers.TryRemove(username, out u);
            }
            try
            {
                return await marketContext.RemoveDataUser(username);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddDataUser(DataUser u)
        {
            if (isDebug)
            {
                return dataUsers.TryAdd(u.username, u);
            }
            try
            {
                return await marketContext.AddDataUser(u);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task RemoveProduct(Product p)
        {
            if (!isDebug)
            {
                try
                {
                    await marketContext.RemoveProduct(p);
                }
                catch (Exception e)
                {
                }
            }
            


    }
}
