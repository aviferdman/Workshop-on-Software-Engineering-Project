using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Notifications;
using TradingSystem.Communication;
using TradingSystem.Notifications;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class PublisherTests
    {
        private User user;
        MarketStores marketStores;
        MarketUsers marketUsers;

        public PublisherTests()
        {
            this.user = new User("UserTests");
            foreach (var s in user.Subscribers)
            {
                s.TestMode = true;
            }
            user.ChangeState(new MemberState(user.Username, user.UserHistory));
            marketStores = MarketStores.Instance;
            marketUsers = MarketUsers.Instance;
            marketUsers.DeleteAll();
            marketStores.DeleteAll();
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public void CheckLoginUserNotifyPurchase()
        {
            user.IsLoggedIn = true;
            Store store = new Store("TestTest", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            Product p = new Product(100, 100, 100);
            store.UpdateProduct(p);
            marketStores.Stores.TryAdd(store.Id, store);
            marketUsers.AddProductToCart(user.Username, p.Id, 5);
            NotificationSubscriber subscriber = user.Subscribers.Where(s => s.SubscriberName.Equals(nameof(EventType.PurchaseEvent))).FirstOrDefault();
            Assert.AreEqual(0, subscriber.Messages.Count);
            marketUsers.PurchaseShoppingCart(user.Username, new BankAccount(1, 1), "054444444", new Address("1", "1", "1", "1"));
            Assert.AreEqual(1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public void CheckLoginUserNotifyCreateShop()
        {
            user.IsLoggedIn = true;
            MarketStores marketStores = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            NotificationSubscriber subscriber = user.Subscribers.Where(s => s.SubscriberName.Equals(nameof(EventType.OpenStoreEvent))).FirstOrDefault();
            Assert.AreEqual(0, subscriber.Messages.Count);
            marketStores.CreateStore("TestStore", user.Username, new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            Assert.AreEqual(1, subscriber.Messages.Count);
        }



        [TestCleanup]
        public void DeleteAll()
        {
            marketUsers.DeleteAll();
            marketStores.DeleteAll();
            this.user = new User("UserTests");
            foreach (var s in user.Subscribers)
            {
                s.TestMode = true;
            }
            user.ChangeState(new MemberState(user.Username, user.UserHistory));
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
        }



    }
}
