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
using static TradingSystem.Business.Market.StoreStates.Manager;
using TradingSystem.PublisherComponent;
using TradingSystem.Notifications;

namespace AcceptanceTests.Tests.LiveNotification
{
    public class AssignNotificationAT
    {
        IMarketBridge marketBridge = SystemContext.Instance.MarketBridge;
        IUserBridge Bridge = SystemContext.Instance.UserBridge;
        PublisherManagement publisherManagement = PublisherManagement.Instance;
        UserInfo founder = new UserInfo("founder", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo owner1 = new UserInfo("owner", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo manager = new UserInfo("manager", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo manager2 = new UserInfo("manager2", "123", null, new Address
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
            publisherManagement.DeleteAll();
            Bridge.Connect();
            Bridge.SignUp(founder);
            Bridge.Login(founder);
            store = marketBridge.OpenShop(new ShopInfo
            (
                "whyy",
                new CreditCard
                (
                    cardNumber: "145236987",
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
            Bridge.SignUp(manager);
            Bridge.Login(manager);
            Bridge.Logout();
            Bridge.SignUp(manager2);
            Bridge.Login(manager2);
            Bridge.Logout();
            Bridge.Login(founder);
            marketBridge.MakeManager("manager", store.Value, "founder");

        }
        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckValidMakeOwner()
        {
            ATSubscriber s = new ATSubscriber("owner");
            publisherManagement.Subscribe("owner", s, EventType.AddAppointmentEvent);
            Assert.IsTrue(marketBridge.MakeOwner("owner", store.Value, "founder"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "AddAppointmentEvent");
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            marketBridge.MakeOwner("owner", store.Value, "founder");
            Bridge.Logout();
            Bridge.Login(owner1);
            Bridge.Logout();
            ATSubscriber s = new ATSubscriber("owner");
            publisherManagement.Subscribe("owner", s, EventType.AddAppointmentEvent);
            Assert.IsFalse(marketBridge.MakeOwner("owner", store.Value, "founder"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s.EventsReceived.Count, 0);
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckMakeOwnerInvalidAssigner()
        {
            ATSubscriber s = new ATSubscriber("owner");
            publisherManagement.Subscribe("owner", s, EventType.AddAppointmentEvent);
            Bridge.Logout();
            Bridge.Login(manager);
            Assert.IsFalse(marketBridge.MakeOwner("owner", store.Value, "manager"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s.EventsReceived.Count, 0);
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckValidMakeManager()
        {
            ATSubscriber s = new ATSubscriber("manager2");
            publisherManagement.Subscribe("manager2", s, EventType.AddAppointmentEvent);
            Assert.IsTrue(marketBridge.MakeManager("manager2", store.Value, "founder"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(manager2);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "AddAppointmentEvent");
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckMakeManagerAlreadyAssigned()
        {
            ATSubscriber s = new ATSubscriber("manager");
            publisherManagement.Subscribe("manager", s, EventType.AddAppointmentEvent);
            Assert.IsFalse(marketBridge.MakeManager("manager", store.Value, "founder"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(manager2);
            Assert.AreEqual(s.EventsReceived.Count, 0);
        }

        //// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckMakeManagerInvalidAssigner()
        {
            ATSubscriber s = new ATSubscriber("manager2");
            publisherManagement.Subscribe("manager2", s, EventType.AddAppointmentEvent);
            Bridge.Logout();
            Bridge.Login(manager);
            Assert.IsFalse(marketBridge.MakeManager("manager2", store.Value, "manager"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(manager2);
            Assert.AreEqual(s.EventsReceived.Count, 0);
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
