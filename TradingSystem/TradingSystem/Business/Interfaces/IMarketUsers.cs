﻿using Moq;
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
        public bool AddMember(String usrname, string guestusername, Guid id);

        bool UpdateProductInShoppingBasket(Guid userId, Guid storeId, Product product, int quantity);
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address);
        public ICollection<IHistory> GetAllHistory(string username);
        public UserHistory GetUserHistory(string username);
        public string logout(string username);
        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(Guid userId);

    }
}