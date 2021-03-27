using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class ShoppingCart
    {
        private Dictionary<Store, ShoppingBasket> _store_shoppingBasket;
        
        public ShoppingCart()
        {
            this._store_shoppingBasket = new Dictionary<Store, ShoppingBasket>();
        }

        public void AddShoppingBasket(Store store, ShoppingBasket shoppingBasket)
        {
            _store_shoppingBasket.Add(store, shoppingBasket);
        }

        public void RemoveShoppingBasket(Store store)
        {
            _store_shoppingBasket.Remove(store);
        }

        public void UpdateShoppingBasket(Store store, ShoppingBasket shoppingBasket)
        {
            try
            {
                _store_shoppingBasket[store] = shoppingBasket;
            }
            catch
            //try to update shopping basket for no existing store
            {

            }
        }
    }
}
