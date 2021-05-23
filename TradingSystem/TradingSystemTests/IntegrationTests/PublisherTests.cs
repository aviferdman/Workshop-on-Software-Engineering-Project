using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Notifications;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class PublisherTests
    {
        private User user;
        private User founderUser;
        private Founder founder;
        private Store store;
        MarketStores marketStores;
        MarketUsers marketUsers;

        [TestInitialize]
        public async Task Initialize()
        {
            ProxyMarketContext.Instance.marketTearDown();
            marketStores = MarketStores.Instance;
            marketUsers = MarketUsers.Instance;
            PublisherManagement.Instance.DeleteAll();
            marketUsers.DeleteAll();
            marketStores.DeleteAll();
            PublisherManagement.Instance.SetTestMode(true);

            this.user = new User("UserTests");
            user.ChangeState(new MemberState(user.Username));
            var dataUser = new DataUser(user.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            await ProxyMarketContext.Instance.AddDataUser(dataUser);
            marketUsers.ActiveUsers.TryAdd(user.Username, user);

            this.store = new Store("Founder", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            founderUser = new User("Founder");
            founderUser.ChangeState(new MemberState(founderUser.Username));
            this.founder = Founder.makeFounder((MemberState)founderUser.State, store);
            store.Founder = founder;
            dataUser = new DataUser(founderUser.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            await ProxyMarketContext.Instance.AddDataUser(dataUser);
            marketUsers.ActiveUsers.TryAdd(founder.Username, founderUser);
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /*
        public PublisherTests()
        {
            marketStores = MarketStores.Instance;
            marketUsers = MarketUsers.Instance;
            PublisherManagement.Instance.DeleteAll();
            marketUsers.DeleteAll();
            marketStores.DeleteAll();
            PublisherManagement.Instance.SetTestMode(true);

            this.user = new User("UserTests");
            user.ChangeState(new MemberState(user.Username));
            var dataUser = new DataUser(user.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            UserManagement.Instance.DataUsers.TryAdd(user.Username, dataUser);
            marketUsers.ActiveUsers.TryAdd(user.Username, user);

            this.store = new Store("Founder", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            founderUser = new User("Founder");
            founderUser.ChangeState(new MemberState(founderUser.Username));
            this.founder = Founder.makeFounder((MemberState)founderUser.State, store);
            store.Founder = founder;
            dataUser = new DataUser(founderUser.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            UserManagement.Instance.DataUsers.TryAdd(founder.Username, dataUser);
            marketUsers.ActiveUsers.TryAdd(founder.Username, founderUser);
            await ProxyMarketContext.Instance.IsDebug(true);
        }
        */

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public async Task CheckLoggedInUserNotifyPurchase()
        {
            founderUser.IsLoggedIn = true;
            Product p = new Product(100, 100, 100);
            store.UpdateProduct(p);
            marketStores.LoadedStores.TryAdd(store.Id, store);
            await marketUsers.AddProductToCart(user.Username, p.Id, 5);
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(founder.Username, EventType.PurchaseEvent);
            PublisherManagement.Instance.FindPublisher(founder.Username).LoggedIn = true;
            Assert.AreEqual(0, subscriber.Messages.Count);
            await marketUsers.PurchaseShoppingCart(user.Username, new CreditCard("1", "1", "1", "1", "1", "1"), "054444444", new Address("1", "1", "1", "1", "1"));
            Assert.AreEqual(1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public async Task CheckNotLoggedInUserStoreNotificationsPurchase()
        {
            var dataUser = new DataUser(founderUser.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = false;
            await ProxyMarketContext.Instance.GetDataUser(founder.Username);
            //UserManagement.Instance.DataUsers[founder.Username] = dataUser;
            Product p = new Product(100, 100, 100);
            store.UpdateProduct(p);
            marketStores.LoadedStores.TryAdd(store.Id, store);
            await marketUsers .AddProductToCart(user.Username, p.Id, 5);
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(founder.Username, EventType.PurchaseEvent);
            Assert.AreEqual(0, subscriber.Messages.Count);
            Assert.AreEqual(0, PublisherManagement.Instance.FindPublisher(founder.Username).Waiting.Count);
            await marketUsers.PurchaseShoppingCart(user.Username, new CreditCard("1", "1", "1", "1", "1", "1"), "054444444", new Address("1", "1", "1", "1", "1"));
            Assert.AreEqual(0, subscriber.Messages.Count);
            Assert.AreEqual(1, PublisherManagement.Instance.FindPublisher(founder.Username).Waiting.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public async Task CheckLoggedInUserNotifyCreateShop()
        {
            user.IsLoggedIn = true;
            MarketStores marketStores = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(user.Username, EventType.OpenStoreEvent);
            PublisherManagement.Instance.FindPublisher(user.Username).LoggedIn = true;
            Assert.AreEqual(0, subscriber.Messages.Count);
            await marketStores.CreateStore("TestStore", user.Username, new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            Assert.AreEqual(1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.Publisher.EventNotification(EventType, string)"/>
        [TestMethod]
        public async Task CheckNotLoggedInUserStoreNotificationsCreateShop()
        {
            //var dataUser = new DataUser(founderUser.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            //dataUser.IsLoggedin = false;
            var du = await ProxyMarketContext.Instance.GetDataUser(user.Username);
            du.IsLoggedin = false;
            //UserManagement.Instance.DataUsers[user.Username] = dataUser;
            MarketStores marketStores = MarketStores.Instance;
            MarketUsers marketUsers = MarketUsers.Instance;
            marketUsers.ActiveUsers.TryAdd(user.Username, user);
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(user.Username, EventType.OpenStoreEvent);
            Assert.AreEqual(0, subscriber.Messages.Count);
            Assert.AreEqual(0, PublisherManagement.Instance.FindPublisher(user.Username).Waiting.Count);
            await marketStores .CreateStore("TestStore", user.Username, new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            Assert.AreEqual(0, subscriber.Messages.Count);
            Assert.AreEqual(1, PublisherManagement.Instance.FindPublisher(user.Username).Waiting.Count);
        }


        [TestCleanup]
        public async Task DeleteAll()
        {
            ProxyMarketContext.Instance.marketTearDown();
            marketStores = MarketStores.Instance;
            marketUsers = MarketUsers.Instance;
            PublisherManagement.Instance.DeleteAll();
            marketUsers.DeleteAll();
            marketStores.DeleteAll();
            PublisherManagement.Instance.SetTestMode(true);

            this.user = new User("UserTests");
            user.ChangeState(new MemberState(user.Username));
            var dataUser = new DataUser(user.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            await ProxyMarketContext.Instance.AddDataUser(dataUser);
            marketUsers.ActiveUsers.TryAdd(user.Username, user);

            this.store = new Store("Founder", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            founderUser = new User("Founder");
            founderUser.ChangeState(new MemberState(founderUser.Username));
            this.founder = Founder.makeFounder((MemberState)founderUser.State, store);
            store.Founder = founder;
            dataUser = new DataUser(founderUser.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            await ProxyMarketContext.Instance.AddDataUser(dataUser);
            marketUsers.ActiveUsers.TryAdd(founder.Username, founderUser);
            ProxyMarketContext.Instance.IsDebug = true;
        }
    }
}
