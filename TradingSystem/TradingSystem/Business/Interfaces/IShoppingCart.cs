using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public interface IShoppingCart
    {
        public IShoppingBasket GetShoppingBasket(IStore store);
        public void AddShoppingBasket(IStore store, IShoppingBasket shoppingBasket);
        public void RemoveShoppingBasket(IStore store);
        public void UpdateShoppingBasket(IStore store, IShoppingBasket shoppingBasket);
        public bool CheckPolicy();
        public BuyStatus Purchase(string username, PaymentMethod method, string clientPhone, Address clientAddress, double paySum);
        public double CalcPaySum();

        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts();

        public bool IsEmpty();
    }
}
