using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private static readonly double WEIGHT2 = 200;
        private static readonly double PRICE2 = 200;
        private User testUser;
        private Store testStore;
        private BankAccount testUserBankAccount;
        private BankAccount testStoreBankAccount;
        private Address testUserAddress;
        private Address testStoreAddress;
        private Product product;

        public UserTests()
        {
            product = new Product(QUANTITY1, WEIGHT1, PRICE1);
            testUserAddress = new Address("1", "1", "1", "1");
            testStoreAddress = new Address("2", "2", "2", "2");
            testUserBankAccount = new BankAccount(1, 1, 1000);
            testUserBankAccount = new BankAccount(2, 2, 2000);
            testUser = new User("testUser");
            testStore = new Store("testStore", testStoreBankAccount, testStoreAddress);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.UpdateProductInShoppingBasket(Store, Product, int)"/>
        [TestMethod]
        public void CheckUpdateNotExistingProduct()
        {

            ShoppingBasket shoppingBasket = testUser.ShoppingCart.GetShoppingBasket(testStore);
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
            ShoppingBasket shoppingBasket = testUser.ShoppingCart.GetShoppingBasket(testStore);
            CheckUpdateNotExistingProduct();
            //now the quantity of product in shopping basket is QUANTITY1
            testUser.UpdateProductInShoppingBasket(testStore, product, QUANTITY2);
            Assert.IsTrue(shoppingBasket.GetProducts().Contains(product));
            Assert.AreEqual(QUANTITY2, shoppingBasket.GetProductQuantity(product));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahse()
        {
            testUser.UpdateProductInShoppingBasket(testStore, product, 1);
            testStore.UpdateProduct(product);
            Assert.IsTrue(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
            Assert.AreEqual(PRICE1, testUser.ShoppingCart.CalcPaySum());
        }

        public bool CheckTotalWeightMoreThan400(Dictionary<Product, int> product_quantity)
        {
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight > 400;
        }

        public bool CheckTotalWeightNoMoreThan400(Dictionary<Product, int> product_quantity)
        {
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight <= 400;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahseWithDiscount()
        {
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Discount discount = new Discount(15);
            Rule rule = new Rule(CheckTotalWeightMoreThan400);
            discount.AddRule(rule);
            testStore.AddDiscount(discount);
            Assert.IsTrue(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
            Assert.AreEqual(PRICE1 * 5 - discount.DiscountValue, testUser.ShoppingCart.CalcPaySum());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahsePolicyIsIllegal()
        {
            Func<Dictionary<Product, int>, bool> f = new Func<Dictionary<Product, int>, bool>(CheckTotalWeightNoMoreThan400);
            Rule r = new Rule(f);
            testStore.AddRule(r);
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Assert.IsFalse(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahseEmptyShoppingCart()
        {
            testStore.UpdateProduct(product);
            Assert.IsFalse(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
        }
    }
}
