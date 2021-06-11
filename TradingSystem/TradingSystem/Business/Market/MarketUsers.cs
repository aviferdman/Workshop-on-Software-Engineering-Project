using Microsoft.EntityFrameworkCore.Storage;
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
        private MarketDAL marketUsersDAL;
        private UsersDAL usersDAL;
        private UserManagement.UserManagement userManagement;
        public ProxyMarketContext proxyMarketContext;
        private ConcurrentDictionary<string, User> activeUsers;
        private HistoryManager historyManager;
        public MarketStores marketStores;
        private static Transaction _transaction = Transaction.Instance;


        public ConcurrentDictionary<string, User> ActiveUsers { get => activeUsers; set => activeUsers = value; }

        public MarketUsers(UserManagement.UserManagement userManagement,HistoryManager historyManager, MarketDAL marketUsersDAL, UsersDAL usersDAL, ProxyMarketContext p)
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            this.historyManager=historyManager;
            this.marketUsersDAL = marketUsersDAL;
            this.usersDAL = usersDAL;
            this.proxyMarketContext = p;
            this.userManagement = userManagement;
        }

        

        public void tearDown()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
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
            Store store = await marketStores.GetStoreById(storeId);
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
            Statistics s = usersDAL.getStatis(DateTime.Now.Date);
            s.guestsNum++;
            proxyMarketContext.saveChanges();
            NotifyAdmins(EventType.Stats);
            return u.Username;
        }

        public void DeleteAll()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
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
            User u;
            if (!activeUsers.TryRemove(guestusername, out u))
                return "user not found in market";
            string loginmang = await userManagement.LogIn(usrname, password);
            if (!loginmang.Equals("success"))
            {
                activeUsers.TryAdd(guestusername,  u);
                return loginmang;
            }
            
            User guest;
            ShoppingCart s;
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
            if(PublisherManagement.Instance.TestMode)
                PublisherManagement.Instance.BecomeLoggedIn(u.Username);
            Statistics stat = usersDAL.getStatis(DateTime.Now.Date);
            if (m is AdministratorState)
            {
                stat.adminNum++;
                await proxyMarketContext.saveChanges();
                return "admin";
            }
            ICollection<Store> stores = await getUserStores(usrname);
            if (stores == null || stores.Count == 0)
            {
                stat.membersNum++;
            }
            else if (stores.Where(s => s.founder.username.Equals(usrname) || s.GetOwner(usrname) != null).Any())
            {
                stat.ownersNum++;
            }
            else
            {
                stat.managersNum++;
            }
            await proxyMarketContext.saveChanges();
            NotifyAdmins(EventType.Stats);
            return "success";
        }

        private void NotifyAdmins(EventType ev)
        {
            foreach (var u in activeUsers.Values)
            {
                if (u.State is AdministratorState)
                {
                    PublisherManagement.Instance.EventNotification(u.Username, ev, "");
                }
            }
        }

        //use case 3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/51
        ///before logout checks  - <see cref="UserManagement.UserManagement.Logout(string)"/> 
        public async Task<string> logout(string username)
        {
            User u;
            bool loginmang = await userManagement.Logout(username);
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
            marketStores.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            if (p.Quantity < quantity || quantity < 1)
                return "product's quantity is insufficient";
            ShoppingBasket basket = await u.ShoppingCart.GetShoppingBasket(found);
            return await basket.addProduct(p, quantity);

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
            marketStores.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            ShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (basket.RemoveProduct(p))
            {
                proxyMarketContext.saveChanges();
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
            marketStores.findStoreProduct(out found, out p, pid);
            if (found == null || p == null)
                return "product doesn't exist";
            if (p.Quantity < quantity || quantity < 1)
                return "product's quantity is insufficient";
            ShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (!basket.TryUpdateProduct(p, quantity))
                return "product not in cart";
            proxyMarketContext.saveChanges();
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
            if (!proxyMarketContext.IsDebug)
            {
                transaction = proxyMarketContext.marketContext.Database.BeginTransaction();
            }
            try
            {
                foreach (KeyValuePair<Guid, int> p in products_added)
                {
                    ans = await AddProductToCart(username, p.Key, p.Value);
                    if (!ans.Equals("product added to shopping basket"))
                    {
                        if (proxyMarketContext.IsDebug)
                        {
                            u.ShoppingCart = c;
                            proxyMarketContext.shoppingCarts.TryRemove(username, out _);
                            proxyMarketContext.shoppingCarts.TryAdd(username, c);
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
                        if (proxyMarketContext.IsDebug)
                        {
                            u.ShoppingCart = c;
                            proxyMarketContext.shoppingCarts.TryRemove(username,out _);
                            proxyMarketContext.shoppingCarts.TryAdd(username, c);
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
                        if (proxyMarketContext.IsDebug)
                        {
                            u.ShoppingCart = c;
                            proxyMarketContext.shoppingCarts.TryRemove(username, out _);
                            proxyMarketContext.shoppingCarts.TryAdd(username, c);
                            return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
                        }
                        throw new Exception();
                    }
                }
                await proxyMarketContext.saveChanges();
                if (!proxyMarketContext.IsDebug)
                {
                    transaction.Commit();
                    transaction.Dispose();

                }
            }
            catch (Exception ex)
            {
                if(!proxyMarketContext.IsDebug)
                {
                    transaction.Rollback();
                }
                return new Result<ShoppingCart>(u.ShoppingCart, true, ans);
            }
            
            return new Result<ShoppingCart>(u.ShoppingCart, false, null);
        }
        public Statistics GetStats()
        {
            return usersDAL.getStatis(DateTime.Now.Date);
        }

        public void CleanMarketUsers()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
        }
    }
}
