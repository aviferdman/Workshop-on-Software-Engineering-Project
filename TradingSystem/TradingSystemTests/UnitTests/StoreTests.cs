using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StoreTests
    {
        //START OF UNIT TESTS

        private readonly string clienId = "usertest";

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckLegalPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            ShoppingBasket shoppingBasket = new ShoppingBasket();
            shoppingBasket.Product_quantity = product_quantity;
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = store.Purchase(shoppingBasket, clienId, clientPhone, address, bankAccount, paySum);
            Assert.AreEqual(true, purchaseStatus.TransactionStatus.Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckNotEnoghtProductQuantityPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 11);
            product_quantity.Add(product2, 22);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            ShoppingBasket shoppingBasket = new ShoppingBasket();
            shoppingBasket.Product_quantity = product_quantity;
            PurchaseStatus purchaseStatus = store.Purchase(shoppingBasket, clienId, clientPhone, address, bankAccount, paySum);
            Assert.AreEqual(false, purchaseStatus.PreConditions);

        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyDiscountsNoDiscounts()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 5);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Assert.AreEqual(0,store.ApplyDiscounts(shoppingBasket));
        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyTwoRelevantDiscounts()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 30);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Discount discount1 = new Discount(100);
            discount1.AddRule(new Rule(MoreThan10Products));
            Discount discount2 = new Discount(200);
            discount1.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(discount1);
            store.AddDiscount(discount2);
            Assert.AreEqual(200, store.ApplyDiscounts(shoppingBasket));
        }

        public bool MoreThan10Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 10;
        }

        public bool MoreThan20Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 20;
        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyOneRelevantDiscount()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 19);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            Discount discount1 = new Discount(100);
            discount1.AddRule(new Rule(MoreThan10Products));
            Discount discount2 = new Discount(200);
            discount2.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(discount1);
            store.AddDiscount(discount2);
            Assert.AreEqual(100, store.ApplyDiscounts(shoppingBasket));
        }


        public bool MoreThan10Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach(KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 10;
        }

        public bool MoreThan20Products(Dictionary<Product, int> product_quantity)
        {
            int count = 0;
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 20;
        }


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }

        //END OF UNIT TESTS
    }
}
