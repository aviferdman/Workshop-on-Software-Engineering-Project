using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using static TradingSystem.Business.Market.StoreStates.Manager;


namespace TradingSystemTests.UnitTests
{
    [TestClass]
    class AssignAndPermissionTests
    {
        static Address address;
        static BankAccount bankAccount;
        Store store;
        Founder founder;
        IManager manager;
        public AssignAndPermissionTests()
        {
            address = new Address("1", "1", "1", "1");
            bankAccount = new BankAccount(1000, 1000);
            store = new Store("testStore", bankAccount, address);
            ICollection<IHistory> historiesf = new List<IHistory>();
            ICollection<IHistory> historieso = new List<IHistory>();
            ICollection<IHistory> historiesm = new List<IHistory>();
            MemberState msf = new MemberState("founder", historiesf);
            MemberState mso = new MemberState("owner", historieso);
            MemberState msm = new MemberState("manager", historiesm);
            founder = Founder.makeFounder(msf, store);
            store.Founder = founder;
            MarketUsers.Instance.MemberStates.TryAdd("owner", mso);
            MarketUsers.Instance.MemberStates.TryAdd("manager", msm);
        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
