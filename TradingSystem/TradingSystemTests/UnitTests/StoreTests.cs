using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class StoreTests
    {
        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckLegalPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            Guid clienId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            //Mock<BankAccount> bankAccount = new Mock<BankAccount>();
            //bankAccount.Setup(ba => ba.CheckEnoughtCurrent(It.IsAny<double>())).Returns(true);
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = store.Purchase(product_quantity, clienId, clientPhone, address, bankAccount, paySum);
            Assert.AreEqual(true, purchaseStatus.TransactionStatus.Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.Purchase(Dictionary{Product, int}, Guid, string, Address, BankAccount, double))"/>
        [TestMethod]
        public void CheckNotEnoghtProductQuantityPurchase()
        {
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            Guid clienId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address address = new Address("1", "1", "1", "1");
            //Mock<BankAccount> bankAccount = new Mock<BankAccount>();
            //bankAccount.Setup(ba => ba.CheckEnoughtCurrent(It.IsAny<double>())).Returns(true);
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            double paySum = 1;
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("2", 20, 20, 20);
            product_quantity.Add(product1, 11);
            product_quantity.Add(product2, 22);
            Store store = new Store("testStore", bankAccount, address);
            store.UpdateProduct(product1);
            store.UpdateProduct(product2);
            PurchaseStatus purchaseStatus = store.Purchase(product_quantity, clienId, clientPhone, address, bankAccount, paySum);
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
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
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
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
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

        /// test for function :<see cref="TradingSystem.Business.Market.Store.ApplyDiscounts(IShoppingBasket))"/>
        [TestMethod]
        public void ApplyOneRelevantDiscount()
        {
            IShoppingBasket shoppingBasket = new ShoppingBasket();
            Product product1 = new Product("1", 10, 10, 10);
            shoppingBasket.UpdateProduct(product1, 19);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
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

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckValidAddProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user.Id), "Product added");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductUnauthorizedUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user1 = new User("user1");
            User user2 = new User("user2");
            Founder founder = new Founder(user1.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user1.Id, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user2.Id), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidPrice()
        {
            Product product1 = new Product("1", 10, 10, -10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user.Id), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.AddProduct(Product, Guid)"/>
        [TestMethod]
        public void CheckAddProductInvalidName()
        {
            Product product1 = new Product("", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            Assert.AreEqual(store.AddProduct(product1, user.Id), "Invalid product");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckValidRemoveProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", user.Id), "Product removed");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidUser()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user1 = new User("user1");
            User user2 = new User("user2");
            Founder founder = new Founder(user1.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user1.Id, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", user2.Id), "Invalid user");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.RemoveProduct(Product)"/>
        [TestMethod]
        public void CheckRemoveProductInvalidPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user1 = new User("user1");
            User user2 = new User("user2");
            Founder founder = new Founder(user1.Id);
            Manager manager = new Manager(user2.Id, founder);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user1.Id, founder);
            personnel.TryAdd(user2.Id, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.RemoveProduct("1", user2.Id), "No Permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckValidEditProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, user.Id), "Product edited");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        public void CheckEditUnavailablwProduct()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("2", product2, user.Id), "Product not in the store");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.EditProduct(string, Product, Guid)"/>
        [TestMethod]
        public void CheckEditNoPermission()
        {
            Product product1 = new Product("1", 10, 10, 10);
            Product product2 = new Product("1", 10, 10, 20);
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user1 = new User("user1");
            User user2 = new User("user2");
            Founder founder = new Founder(user1.Id);
            Manager manager = new Manager(user2.Id, founder);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user1.Id, founder);
            personnel.TryAdd(user2.Id, manager);
            store.Personnel = personnel;
            store.Products.TryAdd("1", product1);
            Assert.AreEqual(store.EditProduct("1", product2, user2.Id), "No Permission");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.GetStoreHistory(Guid)"/>
        [TestMethod]
        public void CheckGetStoreHistoryWithPermission()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user1");
            Founder founder = new Founder(user.Id);
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, founder);
            store.Personnel = personnel;
            History history = store.GetStoreHistory(user.Id);
            Assert.IsNotNull(history);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.GetStoreHistory(Guid)"/>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void CheckGetStoreHistoryWithoutPermission()
        {
            Address address = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(1000, 1000, 1000);
            Store store = new Store("testStore", bankAccount, address);
            User user = new User("user1");
            Manager manager = new Manager(user.Id, new Founder(Guid.NewGuid()));
            ConcurrentDictionary<Guid, StorePermission> personnel = new ConcurrentDictionary<Guid, StorePermission>();
            personnel.TryAdd(user.Id, manager);
            store.Personnel = personnel;
            History history = store.GetStoreHistory(user.Id);
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
