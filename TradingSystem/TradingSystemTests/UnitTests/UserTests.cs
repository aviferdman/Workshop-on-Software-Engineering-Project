﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class UserTests
    {
        private static readonly int QUANTITY1 = 100;
        private static readonly double WEIGHT1 = 100;
        private static readonly double PRICE1 = 100;
        private static readonly int QUANTITY2 = 200;
        private User testUser;
        private Store testStore;
        private BankAccount testStoreBankAccount;
        private Address testStoreAddress;
        private Product product;

        public UserTests()
        {
            product = new Product(QUANTITY1, WEIGHT1, PRICE1);
            testStoreAddress = new Address("2", "2", "2", "2");
            testStoreBankAccount = new BankAccount(2, 2);
            testUser = new User("testUser");
            testStore = new Store("testStore", testStoreBankAccount, testStoreAddress);
        }

        //START UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.User.UpdateProductInShoppingBasket(Store, Product, int)"/>
        [TestMethod]
        public void CheckUpdateNotExistingProduct()
        {

            IShoppingBasket shoppingBasket = testUser.ShoppingCart.GetShoppingBasket(testStore);
            Assert.IsFalse(shoppingBasket.GetProducts().Contains(product));
            Assert.AreEqual(0, shoppingBasket.GetProductQuantity(product));
            testUser.UpdateProductInShoppingBasket(testStore, product, QUANTITY1);
            Assert.IsTrue(shoppingBasket.GetProducts().Contains(product));
            Assert.AreEqual(QUANTITY1, shoppingBasket.GetProductQuantity(product));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.UpdateProductInShoppingBasket(Store, Product, int)"/>
        [TestMethod]
        public void CheckUpdateExistingProduct()
        {
            IShoppingBasket shoppingBasket = testUser.ShoppingCart.GetShoppingBasket(testStore);
            CheckUpdateNotExistingProduct();
            //now the quantity of product in shopping basket is QUANTITY1
            testUser.UpdateProductInShoppingBasket(testStore, product, QUANTITY2);
            Assert.IsTrue(shoppingBasket.GetProducts().Contains(product));
            Assert.AreEqual(QUANTITY2, shoppingBasket.GetProductQuantity(product));
        }



        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

        //END UNIT TESTS

    }
}
