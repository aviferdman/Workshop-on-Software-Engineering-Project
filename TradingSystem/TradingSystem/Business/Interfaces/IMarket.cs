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
    public interface IMarket 
    {
        void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode);
        public string AddGuest();
        public void RemoveGuest(String usrname);
        public bool AddMember(String usrname, string guestusername, Guid id);

        bool UpdateProductInShoppingBasket(Guid userId, Guid storeId, Product product, int quantity);
        public Store CreateStore(string name, string username, BankAccount bank, Address address);
        public ICollection<Store> GetStoresByName(string name);
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address);
        public ICollection<IHistory> GetAllHistory(string username);
        public UserHistory GetUserHistory(string username);
        public StoreHistory GetStoreHistory(string username, Guid storeId);
        public double ApplyDiscounts(string username, Guid storeId);
        public string logout(string username);
        public String AddProduct(ProductData product, Guid storeID, String username);
        public String RemoveProduct(String productName, Guid storeID, String username);
        public String EditProduct(String productName, ProductData details, Guid storeID, String username);
        public String makeOwner(String assigneeName, Guid storeID, String assignerName);
        public String makeManager(String assignee, Guid storeID, String assigner);
        public IDictionary<Guid, IDictionary<Guid, int>> GetShopingCartProducts(Guid userId);

        public String DefineManagerPermissions(String manager, Guid storeID, String assigner, List<Permission> permissions);
    }
}
