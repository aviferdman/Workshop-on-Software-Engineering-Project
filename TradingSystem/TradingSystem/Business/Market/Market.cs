using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public class Market: IMarket
    {
        private ConcurrentDictionary<Guid,IStore> _stores;
        private ConcurrentDictionary<string, User> activeUsers;
        private ConcurrentDictionary<string, IShoppingCart> membersShoppingCarts;
        private static Transaction _transaction = Transaction.Instance;
        private static readonly Lazy<Market>
        _lazy =
        new Lazy<Market>
            (() => new Market());

        public static Market Instance { get { return _lazy.Value; } }

        public ConcurrentDictionary<Guid, IStore> Stores { get => _stores; set => _stores = value; }
        public ConcurrentDictionary<string, User> ActiveUsers { get => activeUsers; set => activeUsers = value; }

        private Market()
        {
            _stores = new ConcurrentDictionary<Guid, IStore>();
            activeUsers = new ConcurrentDictionary<string, User>();
            membersShoppingCarts = new ConcurrentDictionary<string, IShoppingCart>();
        }

        public void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode)
        {
            _transaction.ActivateDebugMode(deliveryAdapter, paymentAdapter, debugMode);
        }

        public bool UpdateProductInShoppingBasket(Guid userId, Guid storeId, Product product, int quantity)
        {
            User user = GetUserById(userId);
            IStore store = GetStoreById(storeId);
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
            User guest = null;
            activeUsers.TryRemove(usrname, out guest);
        }


        ///after login - <see cref="UserManagement.UserManagement.LogIn(string, string, string)"/> 
        public bool AddMember(String usrname, string guestusername, Guid id)
        {
            User u;
            User guest;
            IShoppingCart s;
            if (!activeUsers.TryRemove(guestusername, out u))
                return false;
            string GuidString;
            u.State = new MemberState(u.Id);
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
            u.ChangeState(new MemberState(u.Id));
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

        //use case 22 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/80
        public Store CreateStore(string name, string username, BankAccount bank, Address address)
        {
            User user = GetUserByUserName(username);
            if (typeof(GuestState).IsInstanceOfType(user.State))
                return null;
            Store store = new Store(name, bank, address);
            store.Personnel.TryAdd(user.Id, new Founder(user.Id));
            if (!_stores.TryAdd(store.Id, store))
                return null;
            return store;
        }

        //use case 20 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/78
        public ICollection<Store> GetStoresByName(string name)
        {
            LinkedList<Store> stores = new LinkedList<Store>();
            foreach(Store s in _stores.Values)
            {
                if (s.Name.Equals(name))
                {
                    stores.AddLast(s);
                }
            }
            return stores;
        }

        //use case 11 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address)
        {
            User user = GetUserByUserName(username);
            return user.PurchaseShoppingCart(bank, phone, address);
        }

        //use case 39 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/65
        public History GetAllHistory(string username)
        {
            User user = GetUserByUserName(username);
            return user.State.GetAllHistory();
        }

        //use case 10 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/70
        public History GetUserHistory(string username)
        {
            User user = GetUserByUserName(username);
            Guid userId = user.Id;
            return user.GetUserHistory(userId);
        }

        //use case 38 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/64
        public History GetStoreHistory(string username, Guid storeId)
        {
            User user = GetUserByUserName(username);
            return user.State.GetStoreHistory(storeId);
        }

        //use case 13 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/76
        public double ApplyDiscounts(string username, Guid storeId)
        {
            IStore store = GetStoreById(storeId);
            User user = GetUserByUserName(username);
            return store.ApplyDiscounts(user.ShoppingCart.GetShoppingBasket(store));
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

        private IStore GetStoreById(Guid storeId)
        {
            IStore s=null;
            _stores.TryGetValue(storeId, out s);
            return  s;
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String AddProduct(ProductData productData, Guid storeID, String username)
        {
            Product product = new Product(productData);
            User user = GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AddProduct(product, user.Id);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            User user = GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.RemoveProduct(productName, user.Id);
        }

        //functional requirement 4.1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/17
        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            Product editedProduct = new Product(details);
            User user = GetUserByUserName(username);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.EditProduct(productName, editedProduct, user.Id);
        }

        //use case 5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
        public string AddProductToCart(string username, Guid pid, string pname, int quantity)
        {
            User u;
            if (!activeUsers.TryGetValue(username, out u))
                return "user doesn't exist";
            Product p=null;
            Store found=null;
            foreach (Store s in _stores.Values)
            {
                if(s.Products.TryGetValue(pname,out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }
                    
            }
            if (found == null||p==null)
                return "product doesn't exist";
            if (p.Quantity <= quantity)
                return "product's quantity is insufficient";
            IShoppingBasket basket= u.ShoppingCart.GetShoppingBasket(found);
            return basket.addProduct(p, quantity);

        }

        //functional requirement 4.3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/47
        public String makeOwner(String assigneeName, Guid storeID, String assignerName)
        {
            return AssignMember(assigneeName, storeID, assignerName, AppointmentType.Owner);
        }

        //functional requirement 4.5 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/55
        public String makeManager(String assigneeName, Guid storeID, String assignerName)
        {
            return AssignMember(assigneeName, storeID, assignerName, AppointmentType.Manager);
        }

        //functional requirement 4.6 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/56
        public String DefineManagerPermissions(String managerName, Guid storeID, String assignerName, List<Permission> permissions)
        {
            User assigner = GetUserByUserName(assignerName);
            IStore store;
            Guid managerID;
            try
            {
                managerID = UserManagement.UserManagement.Instance.getIdByUsername(managerName);
            }catch { return "Manager doesn't exist"; }
            Guid assignerID;
            try
            {
                assignerID = UserManagement.UserManagement.Instance.getIdByUsername(assignerName);
            }
            catch { return "Manager doesn't exist"; }
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.DefineManagerPermissions(managerID, assignerID, permissions);
        }

        public String AssignMember(String assigneeName, Guid storeID, String assignerName, AppointmentType type)
        {
            Guid assigneeID;
            try
            {
                assigneeID = UserManagement.UserManagement.Instance.getIdByUsername(assigneeName);
            }
            catch { return "Assignee is not a member"; }
            User assigner = GetUserByUserName(assignerName);
            IStore store;
            if (!_stores.TryGetValue(storeID, out store))
                return "Store doesn't exist";
            return store.AssignMember(assigneeID, assigner, type);
        }

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(Guid userId)
        {
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
            foreach (Store s in _stores.Values)
            {
                if (s.Products.TryGetValue(pname, out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }

            }
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
            foreach (Store s in _stores.Values)
            {
                if (s.Products.TryGetValue(pname, out p))
                {
                    if (p.Id.Equals(pid))
                    {
                        found = s;
                        break;
                    }
                }

            }
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
        public ICollection<Product> findProducts(string keyword, int price_range_low, int price_range_high, int rating, string category)
        {
            List<Product> products = new List<Product>();
            foreach (Store s in _stores.Values)
            {
                products.AddRange(s.findProducts(keyword, price_range_low, price_range_high, rating, category));
            }
            return products;
        }

    }
}
