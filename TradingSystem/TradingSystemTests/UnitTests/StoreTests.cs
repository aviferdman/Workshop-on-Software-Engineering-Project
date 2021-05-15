using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.Business.Market.StoreStates;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StoreTests
    {
        //START OF UNIT TESTS

        private ShoppingCart shoppingCart;
        private IStore store;
        private ShoppingBasket shoppingBasket;
        private User testUser;

        public StoreTests()
        {
            this.testUser = new User("testuser");
            this.shoppingCart = new ShoppingCart(testUser);
            this.store = new Store("tets", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(shoppingCart, store);
            
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, CreditCard, double))"/>
        [TestMethod]
        public async Task CheckLegalPurchaseAsync()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Product product1 = new Product("1", 10, 10, 10, "category");
            Product product2 = new Product("2", 20, 20, 20, "category");
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            shoppingBasket.Product_quantity = product_quantity;
            Store store = new Store("testStore", card, address);
            store.Founder = Founder.makeFounder(new MemberState("userTest"), store);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = await store.Purchase(shoppingBasket, clientPhone, address, card);
            Assert.AreEqual(true, purchaseStatus.TransactionStatus.Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, CreditCard, double))"/>
        [TestMethod]
        public async Task CheckNotEnoghtProductQuantityPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Product product1 = new Product("1", 10, 10, 10, "category");
            Product product2 = new Product("2", 20, 20, 20, "category");
            product_quantity.Add(product1, 11);
            product_quantity.Add(product2, 22);
            Store store = new Store("testStore", card, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            shoppingBasket.Product_quantity = product_quantity;
            PurchaseStatus purchaseStatus = await store.Purchase(shoppingBasket, clientPhone, address, card);
            Assert.AreEqual(false, purchaseStatus.PreConditions);

        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyDiscountsNoDiscounts()
        {
            Product product1 = new Product("1", 10, 10, 10, "category");
            shoppingBasket.UpdateProduct(product1, 5);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Store store = new Store("testStore", card, address);
            store.UpdateProduct(product1);
            Assert.AreEqual(0,store.ApplyDiscounts(shoppingBasket));
        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyTwoRelevantDiscounts()
        {
            Product product1 = new Product("1", 10, 10, 10, "category");
            shoppingBasket.UpdateProduct(product1, 30);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Store store = new Store("testStore", card, address);
            store.SetFounder(Founder.makeFounder(new MemberState("Founder"), store));
            store.UpdateProduct(product1);
            ConditionDiscount discount1 = new ConditionDiscount(new DiscountCalculator(return100));
            discount1.AddRule(new Rule(MoreThan10Products));
            ConditionDiscount discount2 = new ConditionDiscount(new DiscountCalculator(return200));
            discount2.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(store.Founder.Username, discount1);
            store.AddDiscount(store.Founder.Username, discount2);
            Assert.AreEqual(200, store.ApplyDiscounts(shoppingBasket));
        }

        public bool MoreThan10Products(IShoppingBasket shoppingBasket)
        {
            int count = 0;
            var product_quantity = shoppingBasket.GetDictionaryProductQuantity();
            foreach (KeyValuePair<Product, int> p_q in product_quantity)
            {
                count += p_q.Value;
            }
            return count > 10;
        }

        public bool MoreThan20Products(IShoppingBasket shoppingBasket)
        {
            int count = 0;
            var product_quantity = shoppingBasket.GetDictionaryProductQuantity();
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
            Product product1 = new Product("1", 10, 10, 10, "category");
            shoppingBasket.UpdateProduct(product1, 19);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Store store = new Store("testStore", card, address);
            store.SetFounder(Founder.makeFounder(new MemberState("Founder"), store));
            store.UpdateProduct(product1);
            ConditionDiscount discount1 = new ConditionDiscount(new DiscountCalculator(return100));
            discount1.AddRule(new Rule(MoreThan10Products));
            ConditionDiscount discount2 = new ConditionDiscount(new DiscountCalculator(return200));
            discount2.AddRule(new Rule(MoreThan20Products));
            store.AddDiscount(store.Founder.Username, discount1);
            store.AddDiscount(store.Founder.Username, discount2);
            Assert.AreEqual(100, store.ApplyDiscounts(shoppingBasket));
        }

        public double return200(IShoppingBasket shoppingBasket)
        {
            return 200;
        }

        public double return100(IShoppingBasket shoppingBasket)
        {
            return 100;
        }

        [TestCleanup]
        public void DeleteAll()
        {
            this.testUser = new User("testuser");
            this.shoppingCart = new ShoppingCart(testUser);
            this.store = new Store("tets", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(shoppingCart, store);
            Transaction.Instance.DeleteAllTests();
        }
        
        //END OF UNIT TESTS
    }
}
