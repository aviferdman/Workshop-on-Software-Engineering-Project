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

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class HistoryTests
    {

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
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            ICollection<IHistory> userHistory = user.GetUserHistory(user.Username);
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
            user.UpdateProductInShoppingBasket(store, product, 5);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            ICollection<IHistory> userHistory = user.GetUserHistory(user.Username);
            Assert.AreEqual(0, userHistory.Count);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsFalse(!v1.IsErr);
            userHistory = user.GetUserHistory(user.Username);
            Assert.AreEqual(0, userHistory.Count);

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
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            user.GetUserHistory(user.Username);

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
            Store store = marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            ICollection<IHistory> storeHistory = store.GetStoreHistory(user.Username);
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
            Store store = marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            ICollection<IHistory> storeHistory = store.GetStoreHistory(user.Username);
            Assert.IsNotNull(storeHistory);
            Assert.AreEqual(0, storeHistory.Count);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsFalse(!v1.IsErr);
            storeHistory = store.GetStoreHistory(user.Username);
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
            Store store = marketStores.CreateStore("storeTest", user.Username, card, address);
            store.UpdateProduct(product);
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            store.GetStoreHistory("false Username");

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
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            ICollection<IHistory> allHistory = user.State.GetAllHistory();
            Assert.IsNotNull(allHistory);
            Assert.AreEqual(2, allHistory.Count);

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
            user.UpdateProductInShoppingBasket(store, product, 5);
            var v1 = await user.PurchaseShoppingCart(card, "0544444444", address);
            Assert.IsTrue(!v1.IsErr);
            user.State.GetAllHistory();

        }

        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }
    }
}
