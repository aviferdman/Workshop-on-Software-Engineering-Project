using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class User
    {
        private State _state;
        private IShoppingCart _shoppingCart;
        private Guid _id;
        private string username;
        private bool isLoggedIn;
        private UserHistory userHistory;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart();
            this._id = Guid.NewGuid();
            this._state = new GuestState();
            this.username = username;
            this.isLoggedIn = false;
            this.UserHistory = new UserHistory();
        }

        public User()
        {
            this._shoppingCart = new ShoppingCart();
            this._id = Guid.NewGuid();
            this._state = new GuestState();
            this.isLoggedIn = false;
        }

        public Guid Id { get => _id; set => _id = value; }
        internal State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public IShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }
        public UserHistory UserHistory { get => userHistory; set => userHistory = value; }

        public void ChangeState(State state)
        {
            State = state;
        }

        public void UpdateProductInShoppingBasket(IStore store, Product product, int quantity)
        {
            IShoppingBasket shoppingBasket = ShoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        public bool PurchaseShoppingCart(BankAccount bank, string phone, Address address)
        {
            //chcek is not empty and legal policy
            if (ShoppingCart.IsEmpty() || !ShoppingCart.CheckPolicy()) return false;
            double paySum = ShoppingCart.CalcPaySum();
            BuyStatus buyStatus = ShoppingCart.Purchase(_id, bank, phone, address, paySum);
            userHistory.AddHistory(buyStatus.PurchaseStatuses);
            return buyStatus.Status;
        }

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts()
        {
            return _shoppingCart.GetShopingCartProducts();
        }

        public UserHistory GetUserHistory(Guid userId)
        {
            return _state.GetUserHistory(userId);
        }

       
    }
}
