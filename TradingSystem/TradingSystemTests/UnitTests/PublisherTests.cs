using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Notifications;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using TradingSystem.Notifications;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    public class PublisherTests
    {
        private Publisher publisher;
        private NotificationSubscriber subscriber;
        private User user;

        public PublisherTests()
        {
            ProxyMarketContext.Instance.marketTearDown();
            this.publisher = new Publisher("UserTests");
            this.user = new User("UserTests");
            this.subscriber = new NotificationSubscriber(user.Username, true);

        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.TypedPublisher.Subscribe(IObserver)"/>
        [TestMethod]
        public void CheckSubscribe()
        {
            Assert.AreEqual(0, publisher.Observers.Count);
            subscriber.Subscribe(publisher);
            Assert.AreEqual(1, publisher.Observers.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.NotificationSubscriber.Unsubscribe()"/>
        [TestMethod]
        public void CheckUnsubscribe()
        {
            Assert.AreEqual(0, publisher.Observers.Count);
            subscriber.Subscribe(publisher);
            Assert.AreEqual(1, publisher.Observers.Count);
            subscriber.Unsubscribe();
            Assert.AreEqual(0, publisher.Observers.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Notifications.TypedPublisher.EventNotification(String)"/>
        [TestMethod]
        public async Task CheckEventNotification()
        {
            var dataUser = new DataUser(user.Username, "", new Address("1", "1", "1", "1", "1"), "054444444");
            dataUser.IsLoggedin = true;
            await ProxyMarketContext.Instance.AddDataUser(dataUser);
            //UserManagement.Instance.DataUsers.TryAdd(user.Username, dataUser);
            //UserManagement.Instance.DataUsers[user.Username] = dataUser;
            publisher.LoggedIn = true;
            Assert.AreEqual(0, subscriber.Messages.Count);
            subscriber.Subscribe(publisher);
            publisher.EventNotification(EventType.OpenStoreEvent, "Test Message");
            Assert.AreEqual(1, subscriber.Messages.Count);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            this.publisher = new Publisher("UserTests");
            ProxyMarketContext.Instance.marketTearDown();
        }
    }
}
