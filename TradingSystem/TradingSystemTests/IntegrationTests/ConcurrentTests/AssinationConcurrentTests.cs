using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;
using static TradingSystem.Business.Market.StoreStates.Manager;
using System.Threading;
using System.Threading.Tasks;



namespace TradingSystemTests.IntegrationTests.ConcurrentTests
{
    [TestClass]
    public class AssinationConcurrentTests
    {

        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        UserManagement userManagement = UserManagement.Instance;
        Store store;

        public AssinationConcurrentTests()
        {

        }



        [TestInitialize]
        public void Initialize()
        {
            String guestName = marketUsers.AddGuest();
            userManagement.SignUp("founder", "123", null, null,false);
            marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("manager", "123", null, null, false);
            marketUsers.AddMember("manager", "123", guestName);
            guestName = marketUsers.AddGuest();
            userManagement.SignUp("owner", "123", null, null, false);
            marketUsers.AddMember("owner", "123", guestName);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            store = market.CreateStore("testStore", "founder", bankAccount, address);
            market.makeOwner("owner", store.Id, "founder");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AssignMember(Guid, User, AppointmentType)"/>
        [TestMethod]
        [TestCategory("uc33")]
        public void TwoAssignersOneAssignment()
        {
            //Assert.AreEqual(market.makeManager("manager", store.Id, "founder"), "Success");


            String val1 = "";
            String val2 = "";
            String guestId = MarketUsers.Instance.AddGuest();
            Task task1 = Task.Factory.StartNew(() => val1 = market.makeManager("manager", store.Id, "founder"));
            Task task2 = Task.Factory.StartNew(() => val2 = market.makeManager("manager", store.Id, "owner"));
            Task.WaitAll(task1, task2);

            bool check1 = String.Equals("Success", val1);
            bool check2 = String.Equals("Success", val2);

            Assert.IsTrue(store.Managers.ContainsKey("manager"));
            Assert.IsTrue(check1 || check2);
            Assert.IsFalse(check1 && check2);

        }



        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }




    }
}