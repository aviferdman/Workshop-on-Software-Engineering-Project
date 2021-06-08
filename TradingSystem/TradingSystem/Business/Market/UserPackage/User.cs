using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Notifications;
using TradingSystem.DAL;
using TradingSystem.Notifications;

namespace TradingSystem.Business.Market
{

    public class User
    {
        private State _state;
        private ShoppingCart _shoppingCart;
        private string username;
        private bool isLoggedIn;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart(this);
            this._state = new GuestState();
            this.username = username;
            this.isLoggedIn = false;
        }

        public User()
        {
            this._shoppingCart = new ShoppingCart(this);
            this._state = new GuestState();
            this.isLoggedIn = false;
        }

        public State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public ShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }

        public void ChangeState(State state)
        {
            State = state;
        }

        public async Task UpdateProductInShoppingBasket(Store store, Product product, int quantity)
        {
            ShoppingBasket shoppingBasket = await ShoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        public async Task<Result<bool>> PurchaseShoppingCart(PaymentMethod method, string phone, Address address)
        {

            BuyStatus buyStatus = await ShoppingCart.Purchase(method, phone, address);
            string message = "";
            //add information to history
            //empty the shopping cart after a successful purchase
            if (buyStatus.Status)
            {
                MarketDAL.Instance.EmptyShppingCart(username);
            }
            else
            {
                message = "Purchase failed.";
            }
            return new Result<bool>(buyStatus.Status, !buyStatus.Status, message);
        }

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts()
        {
            return _shoppingCart.GetShopingCartProducts();
        }

        public async Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            return await _state.GetUserHistory(username);
        }
    }
}
