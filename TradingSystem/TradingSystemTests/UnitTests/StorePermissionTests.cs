using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StorePermissionTests
    {
        /*
        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        public void FounderAddAppointment()
        {
            Founder founder = new Founder(Guid.NewGuid());
            IStorePermission storePermission = founder.AddAppointment(Guid.NewGuid(), AppointmentType.Owner);
            Assert.IsNotNull(storePermission);  //succeeded and not null
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        public void ManagerAddAppointmentWithPermission()
        {
            Manager manager = new Manager(Guid.NewGuid(), new Founder(Guid.NewGuid()));
            manager.Store_permission.Add(Permission.AppointManger);
            IStorePermission storePermission = manager.AddAppointment(Guid.NewGuid(), AppointmentType.Manager);
            Assert.IsNotNull(storePermission);  //succeeded and not null
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ManagerAddAppointmentWithoutPermission()
        {
            Manager manager = new Manager(Guid.NewGuid(), new Founder(Guid.NewGuid()));
            manager.AddAppointment(Guid.NewGuid(), AppointmentType.Manager);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ManagerAddOwnerAppointmentWithPermission()
        {
            Manager manager = new Manager(Guid.NewGuid(), new Founder(Guid.NewGuid()));
            manager.Store_permission.Add(Permission.AppointManger);
            manager.AddAppointment(Guid.NewGuid(), AppointmentType.Owner);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        public void OwnerAddAppointmentWithPermission()
        {
            Owner owner = new Owner(Guid.NewGuid(), new Founder(Guid.NewGuid()));
            IStorePermission storePermission = owner.AddAppointment(Guid.NewGuid(), AppointmentType.Manager);
            Assert.IsNotNull(storePermission);  //succeeded and not null
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddAppointment(Guid, AppointmentType)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void OwnerAddOwnerAppointmentWithPermission()
        {
            Manager manager = new Manager(Guid.NewGuid(), new Founder(Guid.NewGuid()));
            manager.AddAppointment(Guid.NewGuid(), AppointmentType.Owner);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddPermission(Guid, Permission)"/>
        /// 
        [TestMethod]
        public void AddPermissionForManager()
        {
            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            StorePermission storePermission = new Founder(userId);
            IStorePermission storePermission2 = storePermission.AddAppointment(userId2, AppointmentType.Manager);
            storePermission2.AddPermission(userId, Permission.AppointManger);   //succeeds
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddPermission(Guid, Permission)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AddCloseStorePermissionForManager()
        {
            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            StorePermission storePermission = new Founder(userId);
            IStorePermission storePermission2 = storePermission.AddAppointment(userId2, AppointmentType.Manager);
            storePermission2.AddPermission(userId, Permission.CloseShop);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddPermission(Guid, Permission)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AddPermissionForManagerWithoutAppoitment()
        {
            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            StorePermission storePermission = new Founder(userId);
            IStorePermission storePermission2 = storePermission.AddAppointment(userId2, AppointmentType.Manager);
            storePermission2.AddPermission(Guid.NewGuid(), Permission.AddProduct);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.RemovePermission(Guid, Permission)"/>
        /// 
        [TestMethod]
        public void RemovePermissionForManager()
        {
            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            StorePermission storePermission = new Founder(userId);
            IStorePermission storePermission2 = storePermission.AddAppointment(userId2, AppointmentType.Manager);
            storePermission2.AddPermission(userId, Permission.AppointManger);   //succeeds
            storePermission2.RemovePermission(userId, Permission.AppointManger);    //succeeds
        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddPermission(Guid, Permission)"/>
        /// 
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void RemovePermissionForManagerWithoutAppoitment()
        {
            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            StorePermission storePermission = new Founder(userId);
            IStorePermission storePermission2 = storePermission.AddAppointment(userId2, AppointmentType.Manager);
            storePermission2.AddPermission(userId, Permission.AddProduct);
            storePermission2.RemovePermission(Guid.NewGuid(), Permission.AppointManger);
        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
        */
    }
}
