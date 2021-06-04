using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.UserBridge;
using AcceptanceTests.AppInterface.Data;
using TradingSystem.PublisherComponent;
using TradingSystem.Notifications;

namespace AcceptanceTests.Tests.LiveNotification
{
    public class RemoveOwnerNotificationAT
    {
        IMarketBridge marketBridge = SystemContext.Instance.MarketBridge;
        IUserBridge Bridge = SystemContext.Instance.UserBridge;
        PublisherManagement publisherManagement= PublisherManagement.Instance;
        UserInfo founder= new UserInfo("founder", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo owner1 = new UserInfo("owner1", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo owner2 = new UserInfo("owner2", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo owner3 = new UserInfo("owner3", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });
        ShopId? store;

        [SetUp]
        public void Initialize()
        {
            marketBridge.SetDbDebugMode(true);
            publisherManagement.DeleteAll();
            publisherManagement.TestMode = true;
            Bridge.Connect();
            Bridge.SignUp(founder);
            Bridge.Login(founder);
            store = marketBridge.OpenShop(new ShopInfo
            (
                "whyy",
                new CreditCard
                (
                    cardNumber: "1452369878887888",
                    month: "09",
                    year: "26",
                    holderName: "Nunu Willamp",
                    cvv: "000",
                    holderId: "030301777"
                ),
                new Address
                {
                    State = "Israel",
                    City = "City 2",
                    Street = "Hello",
                    ApartmentNum = "5",
                    ZipCode = "55555",
                })
            );
            Bridge.Logout();
            Bridge.SignUp(owner1);
            Bridge.Login(owner1);
            Bridge.Logout();
            Bridge.SignUp(owner2);
            Bridge.Login(owner2);
            Bridge.Logout();
            Bridge.SignUp(owner3);
            Bridge.Login(owner3);
            Bridge.Logout();
            Bridge.Login(founder);
            marketBridge.MakeOwner("owner1", store.Value, "founder");
            Bridge.Logout();
            Bridge.Login(owner1);
            marketBridge.MakeOwner("owner2", store.Value, "owner1");
            marketBridge.MakeOwner("owner3", store.Value, "owner1");
            Bridge.Logout();
            Bridge.Login(owner1);
            Bridge.Logout();
            Bridge.Login(owner2);
            Bridge.Logout();
            Bridge.Login(owner3);
            Bridge.Logout();
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManagerAsync(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveOwner()
        {
            ATSubscriber s = new ATSubscriber("owner2");
            publisherManagement.Subscribe("owner2", s, EventType.RemoveAppointment);
            Bridge.Login(owner1);
            Assert.IsTrue(marketBridge.RemoveOwner("owner2", store.Value, "owner1"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner2);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "RemoveAppointment");
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveOwnerAsync(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveOwnerChain()
        {

            ATSubscriber s1 = new ATSubscriber("owner1");
            publisherManagement.Subscribe("owner1", s1, EventType.RemoveAppointment);
            ATSubscriber s2 = new ATSubscriber("owner2");
            publisherManagement.Subscribe("owner2", s2, EventType.RemoveAppointment);
            ATSubscriber s3 = new ATSubscriber("owner3");
            publisherManagement.Subscribe("owner3", s3, EventType.RemoveAppointment);
            Assert.IsTrue(marketBridge.RemoveOwner("owner1", store.Value, "founder"));
            Assert.AreEqual(s1.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s1.EventsReceived.Count, 1);
            Assert.AreEqual(s1.EventsReceived.Dequeue().EventProviderName, "RemoveAppointment"); 
            Assert.AreEqual(s2.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner2);
            Assert.AreEqual(s2.EventsReceived.Count, 1);
            Assert.AreEqual(s2.EventsReceived.Dequeue().EventProviderName, "RemoveAppointment");
            Assert.AreEqual(s3.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner3);
            Assert.AreEqual(s3.EventsReceived.Count, 1);
            Assert.AreEqual(s3.EventsReceived.Dequeue().EventProviderName, "RemoveAppointment");
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveOwnerAsync(string, Guid, string)"/>
        [Test]
        public void checkRemoveOwnerInvalidAssigner()
        {
            ATSubscriber s2 = new ATSubscriber("owner2");
            publisherManagement.Subscribe("owner2", s2, EventType.RemoveAppointment);
            Bridge.Login(founder);
            Assert.IsFalse(marketBridge.RemoveOwner("owner2", store.Value, "founder"));
            Assert.AreEqual(s2.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner2);
            Assert.AreEqual(s2.EventsReceived.Count, 0);
        }

        [TearDown]
        public void DeleteAll()
        {
            Bridge.Logout();
            Bridge.Disconnect();
            Bridge.tearDown();
            marketBridge.tearDown();
        }
    }
}
