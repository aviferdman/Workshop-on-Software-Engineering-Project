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

namespace AcceptanceTests.Tests.Market.Appointments
{
    public class AssignAndPermissionAT
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
            Assert.IsTrue(marketBridge.MakeOwner("owner", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckMakeOwnerAlreadyAssigned()
        {
            marketBridge.MakeOwner("owner", store.Value, "founder");
            Assert.IsFalse(marketBridge.MakeOwner("owner", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckMakeOwnerInvalidAssigner()
        {
            Bridge.Logout();
            Bridge.Login(manager);
            Assert.IsFalse(marketBridge.MakeOwner("owner", store.Value, "manager"));
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.IsTrue(marketBridge.MakeOwner("owner", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeOwnerAsync(string, Guid, string)"/>
        [Test]
        public void CheckMakeOwnerNotMember()
        {
            Assert.IsFalse(marketBridge.MakeOwner("no one", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckValidMakeManager()
        {
            Assert.IsTrue(marketBridge.MakeManager("manager2", store.Value, "founder"));
            Assert.IsFalse(marketBridge.MakeManager("manager2", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckMakeManagerAlreadyAssigned()
        {
            Assert.IsFalse(marketBridge.MakeManager("manager", store.Value, "founder"));
        }

        //// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.MakeManger(string, Guid, string)"/>
        [Test]
        public void CheckMakeManagerInvalidAssigner()
        {
            Bridge.Logout();
            Bridge.Login(manager);
            Assert.IsFalse(marketBridge.MakeManager("manager2", store.Value, "manager"));
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.IsTrue(marketBridge.MakeManager("manager2", store.Value, "founder"));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.DefineManagerPermissionsAsync(string, Guid, string, List{TradingSystem.Business.Market.StoreStates.Manager.Permission})"/>
        [Test]
        public void CheckValidDefinePermissions()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.IsTrue(marketBridge.DefineManagerPermissions("manager", store.Value, "founder", permissions));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.DefineManagerPermissionsAsync(string, Guid, string, List{TradingSystem.Business.Market.StoreStates.Manager.Permission})"/>
        [Test]
        public void CheckDefinePermissionsInvalidAssigner()
        {
            Bridge.Logout();
            Bridge.Login(owner1);
            marketBridge.MakeOwner("owner", store.Value, "founder");
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.IsFalse(marketBridge.DefineManagerPermissions("manager", store.Value, "owner", permissions));
        }

        /// test for function :<see cref="TradingSystem.Service.MarketStorePermissionsManagementService.DefineManagerPermissionsAsync(string, Guid, string, List{TradingSystem.Business.Market.StoreStates.Manager.Permission})"/>
        [Test]
        public void CheckDefinePermissionsNoManager()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.Add(Permission.AddProduct);
            Assert.IsFalse(marketBridge.DefineManagerPermissions("manager2", store.Value, "founder", permissions));
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
