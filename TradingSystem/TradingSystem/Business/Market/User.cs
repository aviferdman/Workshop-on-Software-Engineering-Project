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
        private IStorePermission _storePermission;

        public User(string username)
        {
            this._shoppingCart = new ShoppingCart();
            this._id = Guid.NewGuid();
            this._storePermission = new StorePermission(_id);
            this._state = new GuestState(_storePermission);
            this.username = username;
        }

        public Guid Id { get => _id; set => _id = value; }
        internal State State { get => _state; set => _state = value; }
        public string Username { get => username; set => username = value; }
        public ShoppingCart ShoppingCart { get => _shoppingCart; set => _shoppingCart = value; }
        public IStorePermission StorePermission { get => _storePermission; set => _storePermission = value; }

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

        public Store CreateStore(string shopName, BankAccount bank, Address address)
        {
            return _state.CreateStore(shopName, bank, address);
        }

        public History GetAllHistory()
        {
            return _state.GetAllHistory();
        }

        public History GetUserHistory(Guid userId)
        {
            return _state.GetUserHistory(userId);
        }

        public History GetStoreHistory(Guid storeId)
        {
            return _state.GetStoreHistory(storeId);
        }

        public bool AddSubject(Guid storeId, Permission permission, IStorePermission subjectStorePermission)
        {
            return _state.AddSubject(storeId, permission, subjectStorePermission);
        }

        public bool RemoveSubject(Guid storeId, IStorePermission subjectStorePermission)
        {
            return _state.RemoveSubject(storeId, subjectStorePermission);
        }
    }
}
