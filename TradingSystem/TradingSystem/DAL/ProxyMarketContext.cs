﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.UserManagement;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using System.Linq;
using TradingSystem.Business.Market.UserPackage;

namespace TradingSystem.DAL
{
    public class ProxyMarketContext
    {
        private bool isDebug;

        public static string conString = "Data Source=marketDB.db";
        private ConcurrentDictionary<string, DataUser> dataUsers;
        private ConcurrentDictionary<string, RegisteredAdmin> admins;
        private ConcurrentDictionary<string, MemberState> memberStates;
        private ConcurrentDictionary<string, Category> categories;
        private ConcurrentDictionary<Guid, Store> stores;
        private ConcurrentDictionary<string, ShoppingCart> shoppingCarts;
        private HashSet<TransactionStatus> transactionStatuses;
        public bool IsDebug { get => isDebug; set => isDebug = value; }

        private MarketContext marketContext;
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
                return transactionStatuses.Where(t=> t.username.Equals(username)).ToList();
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

        public async Task AddStore(Store store)
        {
            if (!isDebug)
            {
                try
                {
                    await marketContext.AddStore(store);
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                stores.TryAdd(store.Id, store);
            }
        }

        public void findStoreProduct(out Store found, out Product p, Guid pid)
        {
            if (!isDebug)
            {
                try
                {
                    marketContext.findStoreProduct(out found, out p, pid);
                }
                catch (Exception e)
                {
                    found = null;
                    p = null;
                }
            }
            else
            {
                found = null;
                p = null;
            }
        }

        public async Task<ICollection<Product>> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            if (isDebug)
            {
                List<Product> products = new List<Product>();
                Category cat;
                if (category != null)
                {
                    if (categories.TryGetValue(category, out cat))
                    {
                        return cat.getAllProducts(keyword, price_range_low, price_range_high, rating);
                    }
                    else
                        return products;
                }
                foreach (Category c in categories.Values)
                {
                    products.AddRange(c.getAllProducts(keyword, price_range_low, price_range_high, rating));
                }
                return products;
            }
            else
            {
                return await marketContext.findProducts(keyword, price_range_low, price_range_high, rating, category);
            }
            

        }

        public async Task<Category> AddNewCategory(string category)
        {
            if (!isDebug)
            {
                try
                {
                    return await marketContext.AddNewCategory(category);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                Category c;
                if (!categories.TryGetValue(category, out c))
                {
                    c = new Category(category);
                    categories.TryAdd(category, c);
                }
                    
                return c;
            }
        }

        public async Task<ICollection<Store>> GetStoresByName(string name)
        {
            if (!isDebug)
            {
                try
                {
                    return await marketContext.GetStoresByName(name);
                }
                catch (Exception e)
                {
                    return new LinkedList<Store>();
                }
            }
            else
            {
                return stores.Values.Where(s=>s.name.ToLower().Contains(name)).ToList();
            }
        }

        public async Task removeOwner(Owner ownerToRemove)
        {
            if (!isDebug)
            {
                try
                {
                    await marketContext.removeOwner(ownerToRemove);
                }
                catch (Exception e)
                {
                }
            }
        }

        public void removeProductFromCart(ProductInCart productInCart)
        {
            if(!isDebug)
            {
                try
                {
                    marketContext.removeProductFromCart(productInCart);
                }
                catch (Exception e)
                {
                }
            }
        }

        public async Task<ICollection<TransactionStatus>> getStoreHistories(Guid storeId)
        {
            if (isDebug)
            {
                return transactionStatuses.Where(t => t.storeID.Equals(storeId)).ToList();
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
            categories = new ConcurrentDictionary<string, Category>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFAULT_ADMIN", "ADMIN", "0501234566");
            dataUsers.TryAdd("DEFAULT_ADMIN", admin);
            admins.TryAdd("DEFAULT_ADMIN", admin);
            memberStates.TryAdd("DEFAULT_ADMIN", new AdministratorState("DEFAULT_ADMIN"));
            shoppingCarts.TryAdd("DEFAULT_ADMIN", new ShoppingCart("DEFAULT_ADMIN"));
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
            marketContext = MarketContext.Instance;
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            memberStates = new ConcurrentDictionary<string, MemberState>();
            shoppingCarts = new ConcurrentDictionary<string, ShoppingCart>();
            stores = new ConcurrentDictionary<Guid, Store>();
            transactionStatuses = new HashSet<TransactionStatus>();
            categories = new ConcurrentDictionary<string, Category>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFAULT_ADMIN", "ADMIN", "0501234566");
            admins.TryAdd("DEFAULT_ADMIN", admin);
            dataUsers.TryAdd("DEFAULT_ADMIN", admin);
            memberStates.TryAdd("DEFAULT_ADMIN", new AdministratorState("DEFAULT_ADMIN"));
            shoppingCarts.TryAdd("DEFAULT_ADMIN", new ShoppingCart("DEFAULT_ADMIN"));
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

        public void UserTearDown()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            memberStates = new ConcurrentDictionary<string, MemberState>();
            shoppingCarts = new ConcurrentDictionary<string, ShoppingCart>();
            stores = new ConcurrentDictionary<Guid, Store>();
            transactionStatuses = new HashSet<TransactionStatus>();
            categories = new ConcurrentDictionary<string, Category>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFAULT_ADMIN", "ADMIN",  "0501234566");
            dataUsers.TryAdd("DEFAULT_ADMIN", admin);
            admins.TryAdd("DEFAULT_ADMIN", admin);
            shoppingCarts.TryAdd("DEFAULT_ADMIN", new ShoppingCart("DEFAULT_ADMIN"));
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

        public async Task removeManager(Manager manager)
        {
            if (!isDebug)
            {
                try
                {
                    await marketContext.removeManager( manager);
                }
                catch (Exception e)
                {
                }
            }
        }



    }
}
