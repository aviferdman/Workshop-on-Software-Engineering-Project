using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;

namespace TradingSystem.Business.Market
{
    public interface IMarketUsers 
    {
        public string AddGuest();
        public void RemoveGuest(String usrname);
        public string AddMember(String usrname, string password, string guestusername);

        bool UpdateProductInShoppingBasket(string username, Guid storeId, Product product, int quantity);
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address);
        public ICollection<IHistory> GetAllHistory(string username);
        public ICollection<IHistory> GetUserHistory(string username);
        public string logout(string username);
        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(string username);

    }
}
