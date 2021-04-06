﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public interface IMarket 
    {
        public string AddGuest();
        public void RemoveGuest(String usrname);
        public bool AddMember(String usrname);
        public bool login(String usrname);
        public Store CreateStore(string name, string username, BankAccount bank, Address address);
        public ICollection<Store> GetStoresByName(string name);
        public bool PurchaseShoppingCart(string username, BankAccount bank, string phone, Address address);
        public History GetAllHistory(string username);
        public History GetUserHistory(string username);
        public History GetStoreHistory(string username, Guid storeId);
        public double ApplyDiscounts(string username, Guid storeId);
        public bool AddPerssonel(string username, string subjectUsername, Guid storeId, AppointmentType permission);
        public bool RemovePerssonel(string username, string subjectUsername, Guid storeId);
<<<<<<< HEAD
        public void AddProduct(ProductData product, Guid storeID, String username);
        public void RemoveProduct(String productName, Guid storeID, String username);
        public void EditProduct(String productName, ProductData details, Guid storeID, String username);

        public User GetUserByUserName(string username);

        public void DeleteAllTests();
=======
        public String AddProduct(ProductData product, Guid storeID, String username);
        public String RemoveProduct(String productName, Guid storeID, String username);
        public String EditProduct(String productName, ProductData details, Guid storeID, String username);
>>>>>>> dd961858b934b521dd2dbeb4bde92989277f66dd
    }
}
