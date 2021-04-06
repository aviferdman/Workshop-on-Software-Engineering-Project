using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    public class User
    {
        private State _state;
        private ShoppingCart _shoppingCart;
        private Guid _id;
        private string username;
        private bool isLoggedIn;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart();
            this._id = Guid.NewGuid();
            this._state = new GuestState();
            this.username = username;
            this.isLoggedIn = false;
        }

        public User()
        {
            this._shoppingCart = new ShoppingCart();
            this._id = Guid.NewGuid();
            this._state = new GuestState();
            this.isLoggedIn = false;
        }

        public Guid Id { get => _id; set => _id = value; }
        public State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public ShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }

        public void ChangeState(State state)
        {
            State = state;
        }

        public void UpdateProductInShoppingBasket(Store store, Product product, int quantity)
        {
            IShoppingBasket shoppingBasket = ShoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        public bool PurchaseShoppingCart(BankAccount bank, string phone, Address address)
        {
            //chcek is not empty and legal policy
            if (ShoppingCart.IsEmpty() || !ShoppingCart.CheckPolicy()) return false;
            double paySum = ShoppingCart.CalcPaySum();
            return ShoppingCart.Purchase(_id, bank, phone, address, paySum);
        }

     

        public History GetUserHistory(Guid userId)
        {
            return _state.GetUserHistory(userId);
        }

       
    }
}
