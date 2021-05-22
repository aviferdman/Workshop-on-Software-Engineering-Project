using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Payment;
using TradingSystem.DAL;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class HistoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(string)"/>
        [TestMethod]
        public async Task GetUserHistoryWithPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", card, address);
            store.Founder = Founder.makeFounder(new MemberState("userTest"), store);
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            ICollection<IHistory> userHistory = await user.GetUserHistory(user.Username);
            Assert.AreEqual(1, userHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(string)"/>
        [TestMethod]
        public async Task GetUserEmptyHistoryPurcahseFailed()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", card, address);
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            paymentSystem.Setup(p => p.CancelPayment(It.IsAny<string>())).Returns(Task.FromResult("1000"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            ICollection<IHistory> userHistory = await user.GetUserHistory(user.Username);
            Assert.IsNotNull(userHistory);
            int originCount = userHistory.Count;
            //ICollection<IHistory> userHistory = await user.GetUserHistory(user.Username);
            //Assert.AreEqual(0, userHistory.Count);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsFalse(!v1.IsErr);
            userHistory = await user.GetUserHistory(user.Username);
            Assert.AreEqual(originCount, userHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetUserHistory(string)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetUserHistoryWithoutPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", card, address);
            store.Founder = Founder.makeFounder(new MemberState("userTest"), store);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            await user.GetUserHistory(user.Username);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Store)"/>
        [TestMethod]
        public async Task GetStoreHistoryWithPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketUsers market = MarketUsers.Instance;
            MarketStores marketStores = MarketStores.Instance;
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = await marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            ICollection<IHistory> storeHistory = await store.GetStoreHistory(user.Username);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(1, storeHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Store)"/>
        [TestMethod]
        public async Task GetStoreEmptyHistoryPurchaseFailed()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketUsers market = MarketUsers.Instance;
            MarketStores marketStores = MarketStores.Instance;
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = await marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            ICollection<IHistory> storeHistory = await store.GetStoreHistory(user.Username);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(0, storeHistory.Count);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsFalse(!v1.IsErr);
            storeHistory = await store.GetStoreHistory(user.Username);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(0, storeHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetStoreHistory(Store)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetStoreHistoryWithoutPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState memberState = new MemberState(user.Username);
            user.ChangeState(memberState);
            MarketUsers market = MarketUsers.Instance;
            MarketStores marketStores = MarketStores.Instance;
            //market.DeleteAllTests();
            market.ActiveUsers.TryAdd(user.Username, user);
            Store store = await marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            await store.GetStoreHistory("false Username");

        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        public async Task GetAllHistoryWithPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            MemberState adminState = new AdministratorState(user.Username);
            user.ChangeState(adminState);
            Store store = new Store("storeTest", card, address);
            store.Founder = Founder.makeFounder(new MemberState("userTest"), store);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            ICollection<IHistory> allHistory = await user.State.GetAllHistory();
            Assert.IsNotNull(allHistory);
            int originCount = allHistory.Count;
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            allHistory = await user.State.GetAllHistory();
            Assert.AreEqual(originCount + 2, allHistory.Count);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MemberState.GetAllHistory()"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetAllHistoryWithoutPermission()
        {
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Address address = new Address("1", "1", "1", "1", "1");
            Product product = new Product(100, 100, 100);
            User user = new User("testUser");
            Store store = new Store("storeTest", card, address);
            store.Founder = Founder.makeFounder(new MemberState("userTest"), store);
            store.UpdateProduct(product);
            await user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            await user.State.GetAllHistory();

        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
