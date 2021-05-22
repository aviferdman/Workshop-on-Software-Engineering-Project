﻿using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Payment;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Business.Market
{
    public class MarketUsers 
    {
        private static readonly string DEFAULT_ADMIN_USERNAME = "DEFAULT_ADMIN_USERNAME";
        private MarketDAL marketUsersDAL = MarketDAL.Instance;
        private ConcurrentDictionary<string, User> activeUsers;
        private HistoryManager historyManager;
        private static Transaction _transaction = Transaction.Instance;
        private static readonly Lazy<MarketUsers>
        _lazy =
        new Lazy<MarketUsers>
            (() => new MarketUsers());

        public static MarketUsers Instance { get { return _lazy.Value; } }

        public ConcurrentDictionary<string, User> ActiveUsers { get => activeUsers; set => activeUsers = value; }

        private MarketUsers()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            historyManager = HistoryManager.Instance;
        }

        

        public void tearDown()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            historyManager = HistoryManager.Instance;
            marketUsersDAL.teardown();
        }

        public async Task<ICollection<Store>> getUserStores(string usrname)
        {
            return await marketUsersDAL.getMemberStores(usrname);
        }

        public async Task<bool> UpdateProductInShoppingBasket(string userId, Guid storeId, Product product, int quantity)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(UpdateProductInShoppingBasket));
            User user = GetUserById(userId);
            Store store = await MarketStores.Instance.GetStoreById(storeId);
            user.UpdateProductInShoppingBasket(store, product, quantity);
            return true;
        }

        private User GetUserById(string username)
        {
            return activeUsers.Values.Where(u => u.Username.Equals(username)).FirstOrDefault();
        }

        //functional requirement 2.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/10
        public string AddGuest()
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(AddGuest));
            User u = new User();
            string GuidString;
            do
            {
                Guid g = Guid.NewGuid();
                GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                u.Username = GuidString;
            } while (!activeUsers.TryAdd(GuidString, u));
            return u.Username;
        }

        public void DeleteAll()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            historyManager = HistoryManager.Instance;
        }

        //functional requirement 2.2: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/13
        public void RemoveGuest(String usrname)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(RemoveGuest));
            User guest = null;
            activeUsers.TryRemove(usrname, out guest);
        }

        //use case 2 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21
        ///using login to check password - <see cref="UserManagement.UserManagement.LogIn(string, string, string)"/> 
        public async Task<string> AddMember(String usrname, string password, string guestusername)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(AddMember));
            string loginmang = await UserManagement.UserManagement.Instance.LogIn(usrname, password);
            if (!loginmang.Equals("success"))
            {
                return loginmang;
            }
            User u;
            User guest;
            ShoppingCart s;
            if (!activeUsers.TryRemove(guestusername, out u))
                return "user not found in market";
            string GuidString;
            MemberState m= await marketUsersDAL.getMemberState(usrname);
            if (m==null)
            {
                 return "user not found in market";
            }
            u.State = m;
            u.Username = usrname;
            u.IsLoggedIn = true;
            u.ShoppingCart= await marketUsersDAL.getShoppingCart(usrname);
            if (u.ShoppingCart == null)
            {
                return "user not found in market";
            }
            ((ShoppingCart)u.ShoppingCart).User1 = u;
            while (!activeUsers.TryAdd(usrname, u))
            {
                if (activeUsers.TryRemove(usrname, out guest))
                {
                    do
                    {
                        Guid g = Guid.NewGuid();
                        GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");
                        guest.Username = GuidString;
                        ((ShoppingCart)guest.ShoppingCart).User1 = guest;
                    } while (!activeUsers.TryAdd(GuidString, guest));
                }

            };
            PublisherManagement.Instance.BecomeLoggedIn(u.Username);
            return "success";
        }
        //use case 3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/51
        ///before logout checks  - <see cref="UserManagement.UserManagement.Logout(string)"/> 
        public async Task<string> logout(string username)
        {
            User u;
            bool loginmang = await UserManagement.UserManagement.Instance.Logout(username);
            if (!loginmang)
            {
                return null;
            }
            if (!activeUsers.TryRemove(username, out u))
                return null;
            u.ChangeState(new GuestState());
            ((ShoppingCart)u.ShoppingCart).User1 = null;
            u.ShoppingCart = new ShoppingCart(u);
            string GuidString;
            do
            {
                Guid g = Guid.NewGuid();
                GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                u.Username = GuidString;
            } while (!activeUsers.TryAdd(GuidString, u));
            PublisherManagement.Instance.BecomeLoggedOut(username);
            return u.Username;
        }


        //USER FUNCTIONALITY
        //TODO Add all chain to db
        //use case 11 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
        public async Task<Result<bool>> PurchaseShoppingCart(string username, CreditCard bank, string phone, Address address)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(PurchaseShoppingCart));
            User user = GetUserByUserName(username);
            Result<bool> res = await user.PurchaseShoppingCart(bank, phone, address);
            return res;
        }

        //use case 39 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/65
        public Task<ICollection<IHistory>> GetAllHistory(string username)
        {

            User user = GetUserByUserName(username);
            return user.State.GetAllHistory();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public async Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetUserHistory));
            User user = GetUserByUserName(username);
            return await user.GetUserHistory(username);
        }



        public User GetUserByUserName(string username)
        {
            User u = null;
            if (!activeUsers.TryGetValue(username, out u))
            {
                return null;
            }
            return u;
        }




        //use case 5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
        public async  Task<string> AddProductToCart(string username, Guid pid, int quantity)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p = null;
            Store found = null;
            MarketStores.Instance.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            if (p.Quantity <= quantity || quantity < 1)
                return "product's quantity is insufficient";
            ShoppingBasket basket = await u.ShoppingCart.GetShoppingBasket(found);

            return await basket.addProductAsync(p, quantity);

        }


        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(string userId)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetShopingCartProducts));
            User user = GetUserById(userId);
            return user.GetShopingCartProducts();
        }
        //use case 6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/54
        public ShoppingCart viewShoppingCart(string username)
        {
            User u = GetUserByUserName(username);
            if (u == null)
                return null;
            return u.ShoppingCart;
        }
        //use case 8: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/68
        public string RemoveProductFromCart(string username, Guid pid)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p = null;
            Store found = null;
            MarketStores.Instance.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            ShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (basket.RemoveProduct(p))
            {
                ProxyMarketContext.Instance.saveChanges();
                return "product removed from shopping basket";
            }

            return "product isn't in basket";
        }
        //use case 9: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/69
        public string ChangeProductQuanInCart(string username, Guid pid, int quantity)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p = null;
            Store found = null;
            MarketStores.Instance.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            if (p.Quantity <= quantity || quantity < 1)
                return "product's quantity is insufficient";
            ShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (!basket.TryUpdateProduct(p, quantity))
                return "product not in cart";
            ProxyMarketContext.Instance.saveChanges();
            return "product updated";

        }

        //use case 7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/59
        public async Task<Result<ShoppingCart>> editShoppingCart(string username, List<Guid> products_removed, Dictionary<Guid, int> products_added, Dictionary<Guid, int> products_quan)
        {
            User u = GetUserByUserName(username);
            string ans=null;
            if (u == null)
                return new Result<ShoppingCart>(null, true, "user doesn't exist");
            if (products_removed.Intersect<Guid>(products_added.Keys).Count() != 0 ||
               products_removed.Intersect<Guid>(products_quan.Keys).Count() != 0 ||
                products_added.Keys.Intersect<Guid>(products_quan.Keys).Count() != 0)
                return new Result<ShoppingCart>(u.ShoppingCart, true, "lists are not disjoint");
            ShoppingCart c = new ShoppingCart(u.ShoppingCart);
            IDbContextTransaction transaction=null;
            if (!ProxyMarketContext.Instance.IsDebug)
                transaction = MarketContext.Instance.Database.BeginTransaction();
            try
            {
                foreach (KeyValuePair<Guid, int> p in products_added)
                {
                    ans = await AddProductToCart(username, p.Key, p.Value);
                    if (!ans.Equals("product added to shopping basket"))
                    {
                        if (ProxyMarketContext.Instance.IsDebug)
                        {
                            u.ShoppingCart = c;
                            return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
                        }
                        throw new Exception();

                    }
                }
                foreach (Guid p in products_removed)
                {
                    ans = RemoveProductFromCart(username, p);
                    if (!ans.Equals("product removed from shopping basket"))
                    {
                        if (ProxyMarketContext.Instance.IsDebug)
                        {
                            u.ShoppingCart = c;
                            return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
                        }
                        throw new Exception();
                    }

                }
                foreach (KeyValuePair<Guid, int> p in products_quan)
                {
                    ans = ChangeProductQuanInCart(username, p.Key, p.Value);
                    if (!ans.Equals("product updated"))
                    {
                        if (ProxyMarketContext.Instance.IsDebug)
                        {
                            u.ShoppingCart = c;
                            return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
                        }
                        throw new Exception();
                    }
                }
                await ProxyMarketContext.Instance.saveChanges();
                if (!ProxyMarketContext.Instance.IsDebug)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                if(!ProxyMarketContext.Instance.IsDebug)
                    transaction.Rollback();
                return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
            }
            
            return new Result<ShoppingCart>(u.ShoppingCart, false, null);
        }

       

        

        public void CleanMarketUsers()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            historyManager = HistoryManager.Instance;
        }
    }
}
