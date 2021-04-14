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
    public interface IMarketStores 
    {
        void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode);
        public Store CreateStore(string name, string username, BankAccount bank, Address address);
        public ICollection<Store> GetStoresByName(string name);
        public StoreHistory GetStoreHistory(string username, Guid storeId);
        public String AddProduct(ProductData product, Guid storeID, String username);
        public String RemoveProduct(String productName, Guid storeID, String username);
        public String EditProduct(String productName, ProductData details, Guid storeID, String username);
        public String makeOwner(String assigneeName, Guid storeID, String assignerName);
        public String makeManager(String assignee, Guid storeID, String assigner);
        public String DefineManagerPermissions(String manager, Guid storeID, String assigner, List<Permission> permissions);
    }
}