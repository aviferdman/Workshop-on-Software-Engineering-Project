using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface.UserBridge;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using Address = AcceptanceTests.AppInterface.Data.Address;

namespace AcceptanceTests.Tests.LiveNotification
{
    public class RemoveMangerNotificationAT
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

        UserInfo owner1 = new UserInfo("owner1", "123", null, new Address
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

        ShopId? store;

        [SetUp]
        public void Initialize()
        {
            publisherManagement.DeleteAll();
            Bridge.Connect();
            Bridge.SignUp(founder);
            Bridge.Login(founder);
            store = marketBridge.OpenShop(new ShopInfo("whyy", new BankAccount(), new Address
            {
                State = "Israel",
                City = "City 2",
                Street = "Hello",
                ApartmentNum = "5",
                ZipCode = "55555",
            }));
            Bridge.Logout();
            Bridge.SignUp(owner1);
            Bridge.Login(owner1);
            Bridge.Logout();
            Bridge.SignUp(manager);
            Bridge.Login(manager);
            Bridge.Logout();
            Bridge.Login(founder);
            marketBridge.MakeManager("manager", store.Value, "founder");
            marketBridge.MakeOwner("owner1", store.Value, "founder");
            Bridge.Logout();
            Bridge.Login(manager);
            Bridge.Logout();
            Bridge.Login(owner1);
            Bridge.Logout();
            Bridge.Login(founder);
        }


        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManager(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveManager()
        {
            ATSubscriber s = new ATSubscriber("manager");
            publisherManagement.Subscribe("manager", s, EventType.RemoveAppointment);
            Assert.IsTrue(marketBridge.RemoveManager("manager", store.Value, "founder"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(manager);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "RemoveAppointment");
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManager(string, Guid, string)"/>
        [Test]
        public void checkRemoveManagerBadAppointer()
        {
            ATSubscriber s = new ATSubscriber("manager");
            publisherManagement.Subscribe("manager", s, EventType.RemoveAppointment);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.IsFalse(marketBridge.RemoveManager("manager", store.Value, "owner1"));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(manager);
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
