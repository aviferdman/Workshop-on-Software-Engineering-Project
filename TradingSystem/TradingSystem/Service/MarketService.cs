using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystem.Service
{
    class MarketService
    {
        private Market market;

        public void ActivateDebugMode(Mock<DeliveryAdapter> deliveryAdapter, Mock<PaymentAdapter> paymentAdapter, bool debugMode = false)
        {
            market.ActivateDebugMode(deliveryAdapter, paymentAdapter, debugMode);
        }

        public String AddProduct(ProductData product, Guid storeID, String username)
        {
            return market.AddProduct(product, storeID, username);
        }

        public String RemoveProduct(String productName, Guid storeID, String username)
        {
            return market.RemoveProduct(productName, storeID, username);
        }

        public String EditProduct(String productName, ProductData details, Guid storeID, String username)
        {
            return market.EditProduct(productName, details, storeID, username);
        }

        public String makeOwner(String assignee, Guid storeID, String assigner)
        {
            return market.makeOwner(assignee, storeID, assigner);
        }

        public String makeManager(String assignee, Guid storeID, String assigner)
        {
            return market.makeManager(assignee, storeID, assigner);
        }

        public StoreData CreateStore(string name, string username, int accountNumber, int branch, double current, string state, string city, string street, string apartmentNum)
        {
            BankAccount bankAccount = new BankAccount(accountNumber, branch, current);
            Address address = new Address(state, city, street, apartmentNum);
            Store store = market.CreateStore(name, username, bankAccount, address);
            return new StoreData(store);
        }

        public bool PurchaseShoppingCart(string username, int accountNumber, int branch, double current, string phone, string state, string city, string street, string apartmentNum)
        {
            BankAccount bankAccount = new BankAccount(accountNumber, branch, current);
            Address address = new Address(state, city, street, apartmentNum);
            return market.PurchaseShoppingCart(username, bankAccount, phone, address);
        }

        public HistoryData GetAllHistory(string username)
        {
            History history = market.GetAllHistory(username);
            return new HistoryData(history);
        }
        public HistoryData GetUserHistory(string username)
        {
            History history = market.GetUserHistory(username);
            return new HistoryData(history);
        }
        public HistoryData GetStoreHistory(string username, Guid storeId)
        {
            History history = market.GetStoreHistory(username, storeId);
            return new HistoryData(history);
        }
        public double ApplyDiscounts(string username, Guid storeId)
        {
            return this.market.ApplyDiscounts(username, storeId);
        }
    }
}
