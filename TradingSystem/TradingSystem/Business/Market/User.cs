using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Market
{
    class User : State
    {
        private State _state;
        private ShoppingCart _shoppingCart;
        private Guid _id;

        public User()
        {
            this._shoppingCart = new ShoppingCart();
            this._id = new Guid();
            this._state = new GuestState();
        }

        public Guid Id { get => _id; set => _id = value; }
        internal State State { get => _state; set => _state = value; }

        public Address GetAddress()
        {
            throw new NotImplementedException();
        }

        public string GetPhone()
        {
            throw new NotImplementedException();
        }
        public Guid GetBankAccount()
        {
            throw new NotImplementedException();
        }

        public void ChangeState(State state)
        {
            State = state;
        }

        public void UpdateProductInShoppingBasket(Store store, Product product, int quantity)
        {
            ShoppingBasket shoppingBasket = _shoppingCart.GetShoppingBasket(store);
            shoppingBasket.UpdateProduct(product, quantity);
        }

        //use case 11 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
        public bool PurchaseShoppingCart()
        {
            //chcek is not empty and legal policy
            if (_shoppingCart.IsEmpty() || !_shoppingCart.CheckPolicy()) return false;
            double paySum = _shoppingCart.CalcPaySum();
            return _shoppingCart.Purchase(_id, GetBankAccount(), GetPhone(), GetAddress(), paySum);
        }

        public override bool CreateStore(string shopName)
        {
            return _state.CreateStore(shopName);
        }

        public override History GetAllHistory()
        {
            return _state.GetAllHistory();
        }

        public override History GetUserHistory(Guid userId)
        {
            return _state.GetUserHistory(userId);
        }

        public override History GetStoreHistory(Guid storeId)
        {
            return _state.GetStoreHistory(storeId);
        }

        public override bool AddSubject(Guid newManagerId, Guid storeId, Permission permission)
        {
            return _state.AddSubject(newManagerId, storeId, permission);
        }

        public override bool RemoveSubject(Guid newManagerId, Guid storeId)
        {
            return _state.RemoveSubject(newManagerId, storeId);
        }
    }
}
