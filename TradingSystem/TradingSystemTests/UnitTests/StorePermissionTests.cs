using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Interfaces;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StorePermissionTests
    {

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddSubject(Guid, Permission, IStorePermission)"/>
        [TestMethod]
        public void FounderAddManager()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 1);
            storeId_per1.Add(storeId, Permission.Founder);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            Assert.AreEqual(true, clientStorePermission.AddSubject(storeId, Permission.Manager, subjectStorePermission));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddSubject(Guid, Permission, IStorePermission)"/>
        [TestMethod]
        public void ManagerAddManager()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 2);
            storeId_per1.Add(storeId, Permission.Manager);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            Assert.AreEqual(true, clientStorePermission.AddSubject(storeId, Permission.Manager, subjectStorePermission));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.AddSubject(Guid, Permission, IStorePermission)"/>
        [TestMethod]
        public void FailManagerAddOwner()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 2);
            storeId_per1.Add(storeId, Permission.Manager);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            Assert.AreEqual(false, clientStorePermission.AddSubject(storeId, Permission.Owner, subjectStorePermission));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.RemoveSubject(Guid, IStorePermission)"/>
        [TestMethod]
        public void FounderRemoveManager()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 1);
            storeId_per1.Add(storeId, Permission.Founder);
            Dictionary<Guid, int> storeId_hir2 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per2 = new Dictionary<Guid, Permission>();
            storeId_hir2.Add(storeId, 2);
            storeId_per2.Add(storeId, Permission.Manager);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            subjectStorePermission.Store_hierarchy = storeId_hir2;
            subjectStorePermission.Store_permission = storeId_per2;
            Assert.AreEqual(true, clientStorePermission.RemoveSubject(storeId, subjectStorePermission));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.RemoveSubject(Guid, IStorePermission)"/>
        [TestMethod]
        public void ManagerRemoveManager()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 2);
            storeId_per1.Add(storeId, Permission.Manager);
            Dictionary<Guid, int> storeId_hir2 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per2 = new Dictionary<Guid, Permission>();
            storeId_hir2.Add(storeId, 3);
            storeId_per2.Add(storeId, Permission.Manager);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            subjectStorePermission.Store_hierarchy = storeId_hir2;
            subjectStorePermission.Store_permission = storeId_per2;
            Assert.AreEqual(true, clientStorePermission.RemoveSubject(storeId, subjectStorePermission));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.StorePermission.RemoveSubject(Guid, IStorePermission)"/>
        [TestMethod]
        public void FailManagerRemoveOwner()
        {
            Guid clientId = Guid.NewGuid();
            Guid subjectId = Guid.NewGuid();
            Guid storeId = Guid.NewGuid();
            Dictionary<Guid, int> storeId_hir1 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per1 = new Dictionary<Guid, Permission>();
            storeId_hir1.Add(storeId, 3);
            storeId_per1.Add(storeId, Permission.Manager);
            Dictionary<Guid, int> storeId_hir2 = new Dictionary<Guid, int>();
            Dictionary<Guid, Permission> storeId_per2 = new Dictionary<Guid, Permission>();
            storeId_hir2.Add(storeId, 3);
            storeId_per2.Add(storeId, Permission.Owner);

            StorePermission clientStorePermission = new StorePermission(clientId);
            clientStorePermission.Store_hierarchy = storeId_hir1;
            clientStorePermission.Store_permission = storeId_per1;
            StorePermission subjectStorePermission = new StorePermission(subjectId);
            subjectStorePermission.Store_hierarchy = storeId_hir2;
            subjectStorePermission.Store_permission = storeId_per2;
            Assert.AreEqual(false, clientStorePermission.RemoveSubject(storeId, subjectStorePermission));

        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

    }
}
