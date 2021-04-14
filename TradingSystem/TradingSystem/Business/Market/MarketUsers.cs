using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class MarketUsers: IMarketUsers
    {
        private static readonly string DEFAULT_ADMIN_USERNAME = "DEFAULT_ADMIN_USERNAME";

        private ConcurrentDictionary<string, User> activeUsers;
        private ConcurrentDictionary<string, User> adminUsers;
        private ConcurrentDictionary<string, IShoppingCart> membersShoppingCarts;
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
            adminUsers = new ConcurrentDictionary<string, User>();
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
            historyManager = HistoryManager.Instance;
            User defaultAdmin = CreateDefaultAdmin();
            adminUsers.TryAdd(defaultAdmin.Username, defaultAdmin);
        }

        private User CreateDefaultAdmin()
        {
            User user = new User(DEFAULT_ADMIN_USERNAME);
            user.ChangeState(new AdministratorState(user.Id, user.UserHistory));
            return user;
        }

        

        public bool AddSystemAdmin(User admin)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(AddSystemAdmin));
            User user = GetAdminByUserName(admin.Username);
            // not possible two admins with the same username
            if (user != null)
            {
                return false;
            }

            return adminUsers.TryAdd(admin.Username, admin);
        }

        public bool RemoveAdmin(User admin)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(RemoveAdmin));
            User removed;
            // at least one admin for the market
            if (adminUsers.Count < 2)
            {
                return false;
            }

            return adminUsers.TryRemove(admin.Username, out removed);
            
        }

        public bool UpdateProductInShoppingBasket(Guid userId, Guid storeId, Product product, int quantity)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(UpdateProductInShoppingBasket));
            User user = GetUserById(userId);
            IStore store = MarketStores.Instance.GetStoreById(storeId);
            user.UpdateProductInShoppingBasket(store, product, quantity);
            return true;
        }

        private User GetUserById(Guid userId)
        {
            return activeUsers.Values.Where(u => u.Id.Equals(userId)).FirstOrDefault();
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
                u.Username=GuidString;
            } while (!activeUsers.TryAdd(GuidString, u));
            return u.Username;
        }

        //functional requirement 2.2: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/13
        public void RemoveGuest(String usrname)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(RemoveGuest));
            User guest = null;
            activeUsers.TryRemove(usrname, out guest);
        }


        ///after login - <see cref="UserManagement.UserManagement.LogIn(string, string, string)"/> 
        public bool AddMember(String usrname, string guestusername, Guid id)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(AddMember));
            User u;
            User guest;
            IShoppingCart s;
            if (!activeUsers.TryRemove(guestusername, out u))
                return false;
            string GuidString;
            u.State = new MemberState(u.Id, new HashSet<IHistory>());
            u.Id = id;
            u.Username = usrname;
            u.IsLoggedIn = true;
            if (membersShoppingCarts.TryGetValue(usrname, out s))
                u.ShoppingCart = s;
            else
                membersShoppingCarts.TryAdd(usrname, u.ShoppingCart);
            while (!activeUsers.TryAdd(usrname, u))
            {
                if(activeUsers.TryRemove(usrname,out guest))
                {
                    do
                    {
                        Guid g = Guid.NewGuid();
                        GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");
                        guest.Username = GuidString;
                    } while (!activeUsers.TryAdd(GuidString, guest));
                }
                
            };
            return true;
        }

        ///after logout - <see cref="UserManagement.UserManagement.Logout(string)"/> 
        public string logout(string username)
        {
            User u;
            if (!activeUsers.TryRemove(username, out u))
                return null;
            u.Id = Guid.NewGuid();
            u.ChangeState(new MemberState(u.Id, new HashSet<IHistory>()));
            u.ShoppingCart = new ShoppingCart();
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


        //USER FUNCTIONALITY

        //use case 11 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(PurchaseShoppingCart));
            User user = GetUserByUserName(username);
            return user.PurchaseShoppingCart(bank, phone, address);
        }

        //use case 39 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/65
        public ICollection<IHistory> GetAllHistory(string username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetAdminByUserName));
            User user = GetUserByUserName(username);
            return user.State.GetAllHistory();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public ICollection<IHistory> GetUserHistory(string username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetUserHistory));
            User user = GetUserByUserName(username);
            Guid userId = user.Id;
            return user.GetUserHistory(userId);
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

        public User GetAdminByUserName(string username)
        {
            User u = null;
            if (!adminUsers.TryGetValue(username, out u))
            {
                return null;
            }
            return u;
        }


        //use case 5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
        public string AddProductToCart(string username, Guid pid, string pname, int quantity)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p=null;
            Store found=null;
            /*foreach (Store s in _stores.Values)
            {
                if(s.Products.TryGetValue(pname,out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }
                    
            }*/
            if (found == null||p==null)
                return "product doesn't exist";
            if (p.Quantity <= quantity)
                return "product's quantity is insufficient";
            IShoppingBasket basket= u.ShoppingCart.GetShoppingBasket(found);
            return basket.addProduct(p, quantity);

        }

       
        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(Guid userId)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetShopingCartProducts));
            User user = GetUserById(userId);
            return user.GetShopingCartProducts();
        }
        //use case 6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/54
        public IShoppingCart viewShoppingCart(string username)
        {
            User u = GetUserByUserName(username);
            if (u == null)
                return null;
            return u.ShoppingCart;
        }
        //use case 8: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/68
        public string RemoveProductFromCart(string username, Guid pid, string pname)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p = null;
            Store found = null;
            /*foreach (Store s in _stores.Values)
            {
                if (s.Products.TryGetValue(pname, out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }

            }*/
            if (found == null || p == null)
                return "product doesn't exist";
            IShoppingBasket basket = u.ShoppingCart.GetShoppingBasket(found);
            if (basket.RemoveProduct(p))
                return "product removed from shopping basket";
            return "product isn't in basket";
        }
        //use case 9: https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/69
        public string ChangeProductQuanInCart(string username, Guid pid, string pname, int quantity)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p = null;
            Store found = null;
            /*foreach (Store s in _stores.Values)
            {
                if (s.Products.TryGetValue(pname, out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }

            }*/
            if (found == null || p == null)
                return "product doesn't exist";
            if (p.Quantity <= quantity)
                return "product's quantity is insufficient";
            IShoppingBasket basket = u.ShoppingCart.GetShoppingBasket(found);
            basket.UpdateProduct(p, quantity);
            return "product updated";

        }

        //use case 7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/59
        public Result<IShoppingCart> editShoppingCart(string username, Dictionary<Guid,string> products_removed, Dictionary<Guid, KeyValuePair<string,int>> products_added, Dictionary<Guid, KeyValuePair<string, int>> products_quan)
        {
            User u = GetUserByUserName(username);
            string ans;
            if (u == null)
                return new Result<IShoppingCart>(null, true, "user doesn't exist");
            foreach (KeyValuePair<Guid, KeyValuePair<string, int>> p in products_added)
            {
                ans = AddProductToCart(username, p.Key, p.Value.Key, p.Value.Value);
                if (!ans.Equals("product added to shopping basket"))
                    return new Result<IShoppingCart>(u.ShoppingCart, true, ans);
            }
            foreach (KeyValuePair<Guid, string> p in products_removed)
            {
                ans = RemoveProductFromCart(username, p.Key, p.Value);
                if (!ans.Equals("product removed from shopping basket"))
                    return new Result<IShoppingCart>(u.ShoppingCart, true, ans);
            }
            foreach (KeyValuePair<Guid, KeyValuePair<string, int>> p in products_quan)
            {
                ans = ChangeProductQuanInCart(username, p.Key, p.Value.Key, p.Value.Value);
                if (!ans.Equals("product updated"))
                    return new Result<IShoppingCart>(u.ShoppingCart, true,  ans);
            }
            return new Result<IShoppingCart>(u.ShoppingCart, false, null);
        }


    }
}
