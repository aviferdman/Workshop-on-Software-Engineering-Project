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
using Address = AcceptanceTests.AppInterface.Data.Address;

namespace AcceptanceTests.Tests.Market.Appointments
{
    public class RemoveMangerAT
    {
        IMarketBridge marketBridge = SystemContext.Instance.MarketBridge;
        IUserBridge Bridge = SystemContext.Instance.UserBridge;
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
            marketBridge.SetDbDebugMode(true);
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
            Bridge.SignUp(manager);
            Bridge.Login(manager);
            Bridge.Logout();
            Bridge.Login(founder);
            marketBridge.MakeManager("manager", store.Value, "founder");
            marketBridge.MakeOwner("owner1", store.Value, "founder");
        }


        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManagerAsync(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveManager()
        {
            Assert.IsTrue(marketBridge.RemoveManager("manager", store.Value, "founder"));
            Assert.IsFalse(marketBridge.RemoveManager("manager", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManagerAsync(string, Guid, string)"/>
        [Test]
        public void checkRemoveManagerNoManager()
        {
            Assert.IsFalse(marketBridge.RemoveManager("manager2", store.Value, "founder"));
            Assert.IsTrue(marketBridge.RemoveManager("manager", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManagerAsync(string, Guid, string)"/>
        [Test]
        public void checkRemoveManagerBadAppointer()
        {
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.IsFalse(marketBridge.RemoveManager("manager", store.Value, "owner1"));
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.IsTrue(marketBridge.RemoveManager("manager", store.Value, "founder"));
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
