using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Payment;


namespace TradingSystemTests.IntegrationTests.ConcurrentTests
{
    [TestClass]
    public class PurchaseConcurrentTests
    {
        private static readonly int QUANTITY1 = 100;
        private static readonly double WEIGHT1 = 100;
        private static readonly double PRICE1 = 100;
        private User testUser;
        private User secondTestUser;
        private Store testStore;
        private BankAccount testUserBankAccount;
        private BankAccount testSecondUserBankAccount;
        private BankAccount testStoreBankAccount;
        private Address testUserAddress;
        private Address testSecondUserAddress;
        private Address testStoreAddress;
        private Product product;
        private Product oneProduct;

        public PurchaseConcurrentTests()
        {
            product = new Product(QUANTITY1, WEIGHT1, PRICE1);
            oneProduct = new Product(1, WEIGHT1, PRICE1);
            testUserAddress = new Address("1", "1", "1", "1");
            testStoreAddress = new Address("2", "2", "2", "2");
            testSecondUserAddress = new Address("3", "3", "3", "3");
            testUserBankAccount = new BankAccount(1, 1);
            testStoreBankAccount = new BankAccount(2, 2);
            testSecondUserBankAccount = new BankAccount(3, 3);
            testStore = new Store("testStore", testStoreBankAccount, testStoreAddress);
            testUser = new User("testUser");
            MemberState m = new MemberState(testUser.Username);
            Founder f = Founder.makeFounder(m, testStore);
            secondTestUser = new User("secondTestUser");
            testStore.Founder = f;
        }

        [TestMethod]
        public void LastProductTwoCustomersSynchro()
        {
            bool val1 = false;
            bool val2 = false;
            testStore.UpdateProduct(oneProduct);
            testUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            secondTestUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);

            val1 = !testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress).IsErr;
            val2 = !secondTestUser.PurchaseShoppingCart(testSecondUserBankAccount, "0533333333", testSecondUserAddress).IsErr;

            Assert.IsTrue(val1 || val2);
            Assert.IsFalse(val1 && val2); //should return false because one of the purchases must fail.

        }

        [TestMethod]
        public void LastProductTwoCustomers()
        {
            bool val1 = false;
            bool val2 = false;
            testStore.UpdateProduct(oneProduct);
            testUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            secondTestUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            Task task1 = Task.Factory.StartNew(() => val1 = !testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress).IsErr);
            Task task2 = Task.Factory.StartNew(() => val2 = !secondTestUser.PurchaseShoppingCart(testSecondUserBankAccount, "0533333333", testSecondUserAddress).IsErr);
            Task.WaitAll(task1, task2);
            //Console.WriteLine("first: " + val1 + " second: " + val2);
            Assert.IsTrue(val1 || val2);
            Assert.IsFalse(val1 && val2); //should return false because one of the purchases must fail.
        }

        [TestMethod]
        public void LastProductTwoCustomersLooped()
        {
            for (int i = 0; i < 10; i++)
            {
                bool val1 = false;
                bool val2 = false;
                oneProduct.Quantity = 1;
                testStore.UpdateProduct(oneProduct);
                testUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
                secondTestUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
                Task task1 = Task.Factory.StartNew(() => val1 = !testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress).IsErr);
                Task task2 = Task.Factory.StartNew(() => val2 = !secondTestUser.PurchaseShoppingCart(testSecondUserBankAccount, "0533333333", testSecondUserAddress).IsErr);
                Task.WaitAll(task1, task2);
                //Console.WriteLine("first: " + val1 + " second: " + val2);
                Assert.IsTrue(val1 || val2);
                Assert.IsFalse(val1 && val2); //should return false because one of the purchases must fail.
            }
        }
    }
}