using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;
using TradingSystem.Business.UserManagement;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Business.Market
{
    public class MarketUsers : IMarketUsers
    {
        private static readonly string DEFAULT_ADMIN_USERNAME = "DEFAULT_ADMIN_USERNAME";

        private ConcurrentDictionary<string, User> activeUsers;
        private ConcurrentDictionary<string, IShoppingCart> membersShoppingCarts;
        private ConcurrentDictionary<string, MemberState> memberStates;
        private HistoryManager historyManager;
        private static Transaction _transaction = Transaction.Instance;
        private static readonly Lazy<MarketUsers>
        _lazy =
        new Lazy<MarketUsers>
            (() => new MarketUsers());

        public static MarketUsers Instance { get { return _lazy.Value; } }

        public ConcurrentDictionary<string, User> ActiveUsers { get => activeUsers; set => activeUsers = value; }
        public ConcurrentDictionary<string, MemberState> MemberStates { get => memberStates; set => memberStates = value; }

        private MarketUsers()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
            historyManager = HistoryManager.Instance;
            memberStates = new ConcurrentDictionary<string, MemberState>();
        }

        

        public void tearDown()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
            historyManager = HistoryManager.Instance;
            memberStates = new ConcurrentDictionary<string, MemberState>();
        }

        public ICollection<Store> getUserStores(string usrname)
        {
            List < Store> s = new List<Store>();
            MemberState m;
            if (!memberStates.TryGetValue(usrname, out m))
                return s;
            foreach(Store st in m.FounderPrems.Keys)
            {
                s.Add(st);
            }
            foreach (Store st in m.OwnerPrems.Keys)
            {
                s.Add(st);
            }
            foreach (Store st in m.ManagerPrems.Keys)
            {
                s.Add(st);
            }
            return s;
        }

        public bool UpdateProductInShoppingBasket(string userId, Guid storeId, Product product, int quantity)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(UpdateProductInShoppingBasket));
            User user = GetUserById(userId);
            IStore store = MarketStores.Instance.GetStoreById(storeId);
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
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
            historyManager = HistoryManager.Instance;
            memberStates = new ConcurrentDictionary<string, MemberState>();
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
        public string AddMember(String usrname, string password, string guestusername)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(AddMember));
            string loginmang = UserManagement.UserManagement.Instance.LogIn(usrname, password);
            if (!loginmang.Equals("success"))
            {
                return loginmang;
            }
            User u;
            User guest;
            IShoppingCart s;
            if (!activeUsers.TryRemove(guestusername, out u))
                return "user not found in market";
            string GuidString;
            MemberState m;
            if (!memberStates.TryGetValue(usrname, out m))
            {
                DataUser du;
                UserManagement.UserManagement.Instance.DataUsers.TryGetValue(usrname, out du);
                if (du.IsAdmin)
                    m = new AdministratorState(usrname);
                else
                    m = new MemberState(usrname);
                memberStates.TryAdd(usrname, m);
            }
            u.State = m;
            u.Username = usrname;
            u.IsLoggedIn = true;
            if (membersShoppingCarts.TryGetValue(usrname, out s))
                u.ShoppingCart = s;
            else
                membersShoppingCarts.TryAdd(usrname, u.ShoppingCart);
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
        public string logout(string username)
        {
            User u;
            bool loginmang = UserManagement.UserManagement.Instance.Logout(username);
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
        public Result<bool> PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(PurchaseShoppingCart));
            User user = GetUserByUserName(username);
            Result<bool> res = user.PurchaseShoppingCart(bank, phone, address);
            return res;
        }

        //use case 39 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/65
        public ICollection<IHistory> GetAllHistory(string username)
        {

            User user = GetUserByUserName(username);
            return user.State.GetAllHistory();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public ICollection<IHistory> GetUserHistory(string username)
        {
            Logger.Instance.MonitorActivity(nameof(MarketUsers) + " " + nameof(GetUserHistory));
            User user = GetUserByUserName(username);
            string userId = user.Username;
            return user.GetUserHistory(username);
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

        public User GetMemberByUserName(string username)
        {
            User u = null;
            MemberState m;
            if (!activeUsers.TryGetValue(username, out u))
            {
                if (memberStates.TryGetValue(username, out m))
                {
                    u = new User(username);
                    u.ChangeState(m);

                }
                else
                    return null;
            }
            return u;
        }



        //use case 5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
        public string AddProductToCart(string username, Guid pid, int quantity)
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
            IShoppingBasket basket = u.ShoppingCart.GetShoppingBasket(found);
            return basket.addProduct(p, quantity);

        }


        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(string userId)
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
            IShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (basket.RemoveProduct(p))
            {
                if (basket.IsEmpty())
                {
                    u.ShoppingCart.removeBasket(found);
                }
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
            IShoppingBasket basket = u.ShoppingCart.TryGetShoppingBasket(found);
            if (basket == null)
                return "product isn't in basket";
            if (!basket.TryUpdateProduct(p, quantity))
                return "product not in cart";
            return "product updated";

        }

        //use case 7 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/59
        public Result<IShoppingCart> editShoppingCart(string username, List<Guid> products_removed, Dictionary<Guid, int> products_added, Dictionary<Guid, int> products_quan)
        {
            User u = GetUserByUserName(username);
            string ans;
            if (u == null)
                return new Result<IShoppingCart>(null, true, "user doesn't exist");
            if (products_removed.Intersect<Guid>(products_added.Keys).Count() != 0 ||
               products_removed.Intersect<Guid>(products_quan.Keys).Count() != 0 ||
                products_added.Keys.Intersect<Guid>(products_quan.Keys).Count() != 0)
                return new Result<IShoppingCart>(u.ShoppingCart, true, "lists are not disjoint");
            ShoppingCart c = new ShoppingCart((ShoppingCart)u.ShoppingCart);
            foreach (KeyValuePair<Guid, int> p in products_added)
            {
                ans = AddProductToCart(username, p.Key, p.Value);
                if (!ans.Equals("product added to shopping basket"))
                {
                    recover_cart(username, u, c);
                    return new Result<IShoppingCart>(u.ShoppingCart, true, ans);
                }
            }
            foreach (Guid p in products_removed)
            {
                ans = RemoveProductFromCart(username, p);
                if (!ans.Equals("product removed from shopping basket"))
                {
                    recover_cart(username, u, c);
                    return new Result<IShoppingCart>(u.ShoppingCart, true, ans);
                }

            }
            foreach (KeyValuePair<Guid, int> p in products_quan)
            {
                ans = ChangeProductQuanInCart(username, p.Key, p.Value);
                if (!ans.Equals("product updated"))
                {
                    recover_cart(username, u, c);
                    return new Result<IShoppingCart>(u.ShoppingCart, true, ans);
                }
            }
            return new Result<IShoppingCart>(u.ShoppingCart, false, null);
        }

        private void recover_cart(string username, User u, ShoppingCart c)
        {
            u.ShoppingCart = c;
            if (membersShoppingCarts.ContainsKey(u.Username))
            {
                membersShoppingCarts.TryRemove(u.Username, out _);
                membersShoppingCarts.TryAdd(username, c);
            }
        }

        public void CleanMarketUsers()
        {
            activeUsers = new ConcurrentDictionary<string, User>();
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
            historyManager = HistoryManager.Instance;
            memberStates = new ConcurrentDictionary<string, MemberState>();
        }
    }
}
