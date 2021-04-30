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

namespace AcceptanceTests.Tests.Market.Appointments
{
    public class RemoveOwnerAT
    {
        IMarketBridge marketBridge = SystemContext.Instance.MarketBridge;
        IUserBridge Bridge = SystemContext.Instance.UserBridge;
        UserInfo founder= new UserInfo("founder", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
        });

        UserInfo owner1 = new UserInfo("owner1", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
        });

        UserInfo owner2 = new UserInfo("owner2", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
        });

        UserInfo owner3 = new UserInfo("owner3", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
        });
        ShopId? store;

        [SetUp]
        public void Initialize()
        {
            Bridge.Connect();
            Bridge.SignUp(founder);
            Bridge.Login(founder);
            store = marketBridge.OpenShop(new ShopInfo("whyy", new BankAccount(), new Address
            {
                State = "Israel",
                City = "City 2",
                Street = "Hello",
                ApartmentNum = "5",
            }));
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
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveManager(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveOwner()
        {
            Bridge.Login(owner1);
            Assert.IsTrue(marketBridge.RemoveOwner("owner2", store.Value, "owner1"));
            Assert.IsFalse(marketBridge.RemoveOwner("owner2", store.Value, "owner1"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveOwner(string, Guid, string)"/>
        [Test]
        public void checkValidRemoveOwnerChain()
        {
            Assert.IsTrue(marketBridge.RemoveOwner("owner1", store.Value, "founder"));
            Assert.IsFalse(marketBridge.RemoveOwner("owner1", store.Value, "founder"));
            Assert.IsFalse(marketBridge.RemoveOwner("owner2", store.Value, "owner1"));
            Assert.IsFalse(marketBridge.RemoveOwner("owner3", store.Value, "owner1"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveOwner(string, Guid, string)"/>
        [Test]
        public void checkRemoveOwnerDoesntExist()
        {
            Bridge.Login(owner1);
            Assert.IsFalse(marketBridge.RemoveOwner("no one", store.Value, "owner1"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.RemoveOwner(string, Guid, string)"/>
        [Test]
        public void checkRemoveOwnerInvalidAssigner()
        {
            Bridge.Login(founder);
            Assert.IsFalse(marketBridge.RemoveOwner("owner2", store.Value, "founder"));
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.IsTrue(marketBridge.RemoveOwner("owner2", store.Value, "owner1"));
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
