using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Communication;
using TradingSystem.Notifications;

namespace TradingSystemTests.UnitTests
{
    [TestClass]
    public class TypedPublisherTests
    {
        private TypedPublisher publisher;
        private NotificationSubscriber subscriber;
        private User user;

        public TypedPublisherTests()
        {
            this.publisher = new TypedPublisher("PublisherTest");
            this.user = new User("UserTests");
            this.subscriber = new NotificationSubscriber(user, user.Username, true);
            Mock<ICommunicate> communicateMock = new Mock<ICommunicate>();
            communicateMock.Setup(c => c.SendMessage(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            subscriber.Communicate = communicateMock.Object;

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
        public void CheckEventNotification()
        {
            Assert.AreEqual(0, subscriber.Messages.Count);
            subscriber.Subscribe(publisher);
            publisher.EventNotification("Test Message");
            Assert.AreEqual(1, subscriber.Messages.Count);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            this.publisher = new TypedPublisher("PublisherTest");
        }
    }
}
