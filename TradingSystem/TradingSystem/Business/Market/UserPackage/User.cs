using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.Histories;

namespace TradingSystem.Business.Market
{
    public class User
    {
        private State _state;
        private IShoppingCart _shoppingCart;
        private string username;
        private bool isLoggedIn;
        private ICollection<IHistory> userHistory;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart();
            this._state = new GuestState();
            this.username = username;
            this.isLoggedIn = false;
            this.UserHistory = new HashSet<IHistory>();
        }

        public User()
        {
            this._shoppingCart = new ShoppingCart();
            this._state = new GuestState();
            this.isLoggedIn = false;
        }

        public State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public IShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }
        public ICollection<IHistory> UserHistory { get => userHistory; set => userHistory = value; }

        public void ChangeState(State state)
        {
            State = state;
        }

        public void UpdateProductInShoppingBasket(IStore store, Product product, int quantity)
        {
            IShoppingBasket shoppingBasket = ShoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        public bool PurchaseShoppingCart(PaymentMethod method, string phone, Address address)
        {
            //chcek is not empty and legal policy
            if (ShoppingCart.IsEmpty() || !ShoppingCart.CheckPolicy()) return false;
            double paySum = ShoppingCart.CalcPaySum();
            BuyStatus buyStatus = ShoppingCart.Purchase(username, method, phone, address, paySum);
            foreach (var p in buyStatus.PurchaseStatuses)
            {
                var h = new TransactionHistory(p);
                userHistory.Add(h);
                HistoryManager.Instance.AddUserHistory(h);
            }
            return buyStatus.Status;
        }

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts()
        {
            return _shoppingCart.GetShopingCartProducts();
        }

        public ICollection<IHistory> GetUserHistory(string username)
        {
            return _state.GetUserHistory(username);
        }

       
    }
}
