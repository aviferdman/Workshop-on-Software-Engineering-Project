﻿using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Payment;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market
{
    public interface IMarketStores 
    {
        void ActivateDebugMode(Mock<ExternalDeliverySystem> deliveryAdapter, Mock<ExternalPaymentSystem> paymentAdapter, bool debugMode);
        public Store CreateStore(string name, string username, CreditCard bank, Address address);
        public ICollection<Store> GetStoresByName(string name);
        public ICollection<IHistory> GetStoreHistory(string username, Guid storeId);
        public Result<Product> AddProduct(ProductData product, Guid storeID, String username);
        public String RemoveProduct(Guid productID, Guid storeID, String username);
        public String EditProduct(Guid productID, ProductData details, Guid storeID, String username);
        public String makeOwner(String assigneeName, Guid storeID, String assignerName);
        IStore GetStoreById(Guid storeId);
        public String makeManager(String assignee, Guid storeID, String assigner);
        public String DefineManagerPermissions(String managerName, Guid storeID, String assignerName, List<Permission> permissions);
        public String RemoveManager(String managerName, Guid storeID, String assignerName);
        public String GetInfo(Guid storeID, String username);
        public String GetInfoSpecific(Guid storeID, String workerName, String username);
        public String RemoveOwner(String ownerName, Guid storeID, String assignerName);
    }
}
