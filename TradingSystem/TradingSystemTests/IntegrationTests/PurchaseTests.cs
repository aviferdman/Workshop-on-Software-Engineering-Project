
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class PurchaseTests
    {
        private static readonly int QUANTITY1 = 100;
        private static readonly double WEIGHT1 = 100;
        private static readonly double PRICE1 = 100;
        private User testUser;
        private IStore testStore;
        private BankAccount testUserBankAccount;
        private BankAccount testStoreBankAccount;
        private Address testUserAddress;
        private Address testStoreAddress;
        private Product product;
        private ShoppingCart shoppingCart;
        private ShoppingBasket shoppingBasket;

        public PurchaseTests()
        {
            product = new Product(QUANTITY1, WEIGHT1, PRICE1);
            testUserAddress = new Address("1", "1", "1", "1");
            testStoreAddress = new Address("2", "2", "2", "2");
            testUserBankAccount = new BankAccount(1, 1);
            testStoreBankAccount = new BankAccount(2, 2);
            testUser = new User("testUser");
            testStore = new Store("testStore", testStoreBankAccount, testStoreAddress);
            this.shoppingCart = new ShoppingCart();
            this.shoppingBasket = new ShoppingBasket(shoppingCart, testStore);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahseWithDiscount()
        {
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Discount discount = new Discount(15);
            IRule rule = new Rule(CheckTotalWeightMoreThan400);
            discount.AddRule(rule);
            testStore.AddDiscount(discount);
            Assert.AreEqual(PRICE1 * 5 - discount.DiscountValue, testUser.ShoppingCart.CalcPaySum());
            Assert.IsTrue(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahseUpdatedQuantityAtStore()
        {
            int originQuantity = product.Quantity;
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            bool successPurchase = testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress);
            Assert.AreEqual(successPurchase, true);
            Assert.AreEqual(originQuantity - 5, product.Quantity);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckIllegalPurcahseStoreQuantityRemains()
        {
            int originQuantity = product.Quantity;
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>())).Returns(new Guid());
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            bool successPurchase = testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress);
            Assert.AreEqual(successPurchase, false);
            Assert.AreEqual(originQuantity, product.Quantity);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void CheckLegalPurcahsePolicyIsIllegal()
        {
            Func<Dictionary<Product, int>, bool> f = new Func<Dictionary<Product, int>, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
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

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, PaymentMethod, Guid, BankAccount, double, IShoppingBasket)"/>
        [TestMethod]
        public void CheckLegalTransaction()
        {
            Transaction transaction = Transaction.Instance;
            TransactionStatus transactionStatus = transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserBankAccount, testStore.GetId(), testStoreBankAccount, 1, shoppingBasket);
            Assert.AreEqual(transactionStatus.Status, true);
            Assert.AreEqual(transactionStatus.DeliveryStatus.Status, true);
            Assert.AreEqual(transactionStatus.PaymentStatus.Status, true);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, BankAccount, Guid, BankAccount, double)"/>
        [TestMethod]
        public void CheckUnAvailablePaymentTransaction()
        {
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>())).Returns(new Guid());
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            TransactionStatus transactionStatus = transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserBankAccount, testStore.GetId(), testStoreBankAccount, 1, shoppingBasket);
            Assert.AreEqual(transactionStatus.Status, false);
            Assert.IsNull(transactionStatus.DeliveryStatus);
            Assert.AreEqual(transactionStatus.PaymentStatus.Status, false);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, BankAccount, Guid, BankAccount, double)"/>
        [TestMethod]
        public void CheckUnAvailableDeliveryTransaction()
        {
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>())).Returns(Guid.NewGuid());
            Mock<ExternalDeliverySystem> deliverySystem = new Mock<ExternalDeliverySystem>();
            deliverySystem.Setup(p => p.CreateDelivery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid());
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            transaction.DeliveryAdapter.SetDeliverySystem(deliverySystem.Object);
            TransactionStatus transactionStatus = transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserBankAccount, testStore.GetId(), testStoreBankAccount, 1, shoppingBasket);
            Assert.AreEqual(transactionStatus.Status, false);
            Assert.AreEqual(transactionStatus.DeliveryStatus.Status, false);
            Assert.AreEqual(transactionStatus.PaymentStatus.Status, true);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Store.CancelTransaction(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckCancelTransactionUpdateQuantity()
        {
            int originalQuantity = product.Quantity;
            testStore.UpdateProduct(product);
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            product_quantity.Add(product, 10);
            testStore.CancelTransaction(product_quantity);
            Assert.AreEqual(product.Quantity, originalQuantity + 10);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void SucceessPurchaseBasketBecomesEmpty()
        {
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
            Assert.IsTrue(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
            Assert.IsTrue(testUser.ShoppingCart.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void FailedPurchaseBasketRemainsTheSame()
        {
            testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>())).Returns(new Guid());
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
            Assert.IsFalse(testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress));
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(BankAccount, string, Address)"/>
        [TestMethod]
        public void LastProductTwoCustomersSynchro()
        {
            bool val1 = false;
            bool val2 = false;
            Product oneProduct = new Product(1, 100, 100);
            User secondTestUser = new User("second test user");
            testStore.UpdateProduct(oneProduct);
            testUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            secondTestUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            val1 = testUser.PurchaseShoppingCart(testUserBankAccount, "0544444444", testUserAddress);
            val2 = secondTestUser.PurchaseShoppingCart(new BankAccount(2, 2), "0533333333", new Address("2", "2", "2", "2")) ;
            Assert.IsTrue(val1 || val2);
            Assert.IsFalse(val1 && val2); //should return false because one of the purchases must fail.

        }

            private bool CheckTotalWeightMoreThan400(Dictionary<Product, int> product_quantity)
        {
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight > 400;
        }

        private bool CheckTotalWeightNoMoreThan400(Dictionary<Product, int> product_quantity)
        {
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight <= 400;
        }
        
        [TestCleanup]
        public void DeleteAll()
        {
            testStore = new Store("testStore", testStoreBankAccount, testStoreAddress);
            this.shoppingCart = new ShoppingCart();
            this.shoppingBasket = new ShoppingBasket(shoppingCart, testStore);
            Transaction.Instance.DeleteAllTests();
        }
    }
}
