using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using TradingSystem.Service;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class BidTests
    {
        int QUANTITY = 5;
        int BID_PRICE = 50;
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        MarketStores marketStores = MarketStores.Instance;
        UserManagement userManagement = UserManagement.Instance;
        PublisherManagement publisherManagement;
        Store store;
        User customer;
        User owner;
        Product product;

        public  BidTests()
        {
            PublisherManagement.Instance.DeleteAll();
            marketUsers.CleanMarketUsers();
            marketStores.DeleteAll();
            publisherManagement = PublisherManagement.Instance;
            publisherManagement.SetTestMode(true);
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("FounderTest1", "123", null, null);
            marketUsers.AddMember("FounderTest1", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("ManagerTest1", "123", null, null);
            marketUsers.AddMember("ManagerTest1", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("OwnerTest1", "123", null, null);
            marketUsers.AddMember("OwnerTest1", "123", guestName);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            store = market.CreateStore("testStore", "FounderTest1", bankAccount, address);
            market.makeOwner("OwnerTest1", store.Id, "FounderTest1");
            owner = marketUsers.ActiveUsers.GetOrAdd("OwnerTest1", owner);
            customer = marketUsers.ActiveUsers.GetOrAdd("ManagerTest1", customer);
            product = new Product(100, 100, 100);
            var shoppingCart = new ShoppingCart(customer); 
            customer.UpdateProductInShoppingBasket(store, product, QUANTITY);
            store.UpdateProduct(product);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.CustomerRequestBid(string, Guid, Guid, double)"/>
        [TestMethod]
        public void TestRequestPurcahse()
        {
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(owner.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            marketStores.CustomerRequestBid(customer.Username, store.Id, product.Id, BID_PRICE);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public void TestOwnerWithPermissionAcceptBid()
        {
            double originPrice = store.CalcPrice(customer.Username, customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(product.Price * QUANTITY, originPrice);
            marketStores.OwnerAcceptBid(owner.Username, customer.Username, store.Id, product.Id, BID_PRICE);
            double bidPrice = store.CalcPrice(customer.Username, customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(BID_PRICE * QUANTITY, bidPrice);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public void TestOwnerWithoutPermissionAcceptBid()
        {
            double originPrice = store.CalcPrice(customer.Username, customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(product.Price * QUANTITY, originPrice);
            marketStores.OwnerAcceptBid("blahblahblah", customer.Username, store.Id, product.Id, BID_PRICE);
            double bidPrice = store.CalcPrice(customer.Username, customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(originPrice, bidPrice);
        }

        /// test for function :<see cref="TradingSystem.Service.MarketShoppingCartService.OwnerAnswerBid(string, TradingSystem.Service.Answer, string, Guid, Guid, double)">
        [TestMethod]
        public void TestOwnerAnswerDenyBid()
        {            
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(customer.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            MarketShoppingCartService.Instance.OwnerAnswerBid(owner.Username, Answer.Deny, customer.Username, store.Id, product.Id);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Service.MarketShoppingCartService.OwnerAnswerBid(string, TradingSystem.Service.Answer, string, Guid, Guid, double)">
        [TestMethod]
        public void TestOwnerAnswerNewBid()
        {
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(customer.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            MarketShoppingCartService.Instance.OwnerAnswerBid(owner.Username, Answer.Deny, customer.Username, store.Id, product.Id, 75);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            UserManagement.Instance.DeleteUser("FounderTest1");
            UserManagement.Instance.DeleteUser("ManagerTest1");
            UserManagement.Instance.DeleteUser("OwnerTest1");
            PublisherManagement.Instance.DeleteAll();
            marketUsers.CleanMarketUsers();
            marketStores.DeleteAll();
        }
    }
}
