
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.Business.Payment;
using TradingSystem.DAL;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class PurchaseTests
    {
        private static readonly double DISCOUNT_VALUE = 15;
        private static readonly int QUANTITY1 = 100;
        private static readonly double WEIGHT1 = 100;
        private static readonly double PRICE1 = 100;
        private User testUser;
        private Store testStore;
        private CreditCard testUserCreditCard;
        private CreditCard testStoreCreditCard;
        private Address testUserAddress;
        private Address testStoreAddress;
        private Product product;
        private ShoppingCart shoppingCart;
        private ShoppingBasket shoppingBasket;

        public PurchaseTests()
        {
            product = new Product(QUANTITY1, WEIGHT1, PRICE1);
            testUserAddress = new Address("1", "1", "1", "1", "1");
            testStoreAddress = new Address("2", "2", "2", "2", "2");
            testUserCreditCard = new CreditCard("1", "1", "1", "1", "1", "1");
            testStoreCreditCard = new CreditCard("1", "1", "1", "1", "1", "1");
            testUser = new User("testUser");
            Store s = new Store("testStore", testStoreCreditCard, testStoreAddress);
            s.Founder = Founder.makeFounder(new MemberState("userTest"), s);
            testStore = s;
            this.shoppingCart = new ShoppingCart(testUser);
            this.shoppingBasket = new ShoppingBasket(shoppingCart, testStore);
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahseWithDiscount()
        {
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            ConditionDiscount discount = new ConditionDiscount(new DiscountCalculator(return15));
            IRule rule = new Rule(CheckTotalWeightMoreThan400);
            discount.AddRule(rule);
            testStore.AddDiscount(testStore.GetFounder().Username, discount);
            Assert.AreEqual(PRICE1 * 5 - DISCOUNT_VALUE, testUser.ShoppingCart.CalcPaySum());
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsTrue(!v1.IsErr);
        }

        private double return15(ShoppingBasket arg)
        {
            return DISCOUNT_VALUE;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahseUpdatedQuantityAtStore()
        {
            int originQuantity = product.Quantity;
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            bool successPurchase = !v1.IsErr;
            Assert.AreEqual(successPurchase, true);
            Assert.AreEqual(originQuantity - 5, product.Quantity);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckIllegalPurcahseStoreQuantityRemains()
        {
            int originQuantity = product.Quantity;
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            bool successPurchase = !v1.IsErr;
            Assert.AreEqual(successPurchase, false);
            Assert.AreEqual(originQuantity, product.Quantity);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckIllegalPurcahseUserRefund()
        {
            Logger.Instance.CleanLogs();
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            Assert.AreEqual(0, Logger.Instance.Activities.Count);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            bool successPurchase = !v1.IsErr;
            Assert.AreEqual(successPurchase, false);
            bool existsRefund = Logger.Instance.Activities.Where(activity => activity.Contains("CancelTransaction")).Any();
            Assert.IsTrue(existsRefund);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahsePolicyIsIllegal()
        {
            Func<ShoppingBasket, bool> f = new Func<ShoppingBasket, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
            testStore.AddRule(testStore.GetFounder().Username, r);
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsFalse(!v1.IsErr);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahseEmptyShoppingCart()
        {
            testStore.UpdateProduct(product);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsFalse(!v1.IsErr);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, PaymentMethod, Guid, CreditCard, double, IShoppingBasket)"/>
        [TestMethod]
        public async Task CheckLegalTransaction()
        {
            Transaction transaction = Transaction.Instance;
            TransactionStatus transactionStatus = await transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserCreditCard, testStore.GetId(), testStore.name, testStoreCreditCard, 1, shoppingBasket);
            Assert.AreEqual(transactionStatus.Status, true);
            Assert.AreEqual(transactionStatus.DeliveryStatus.Status, true);
            Assert.AreEqual(transactionStatus.PaymentStatus.Status, true);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, CreditCard, Guid, CreditCard, double)"/>
        [TestMethod]
        public async Task CheckUnAvailablePaymentTransaction()
        {
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            TransactionStatus transactionStatus = await transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserCreditCard, testStore.GetId(), testStore.name, testStoreCreditCard, 1, shoppingBasket);
            Assert.AreEqual(transactionStatus.Status, false);
            Assert.IsNull(transactionStatus.DeliveryStatus);
            Assert.AreEqual(transactionStatus.PaymentStatus.Status, false);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Transaction.ActivateTransaction(string, string, double, Address, Address, CreditCard, Guid, CreditCard, double)"/>
        [TestMethod]
        public async Task CheckUnAvailableDeliveryTransaction()
        {
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            Mock<ExternalDeliverySystem> deliverySystem = new Mock<ExternalDeliverySystem>();
            deliverySystem.Setup(p => p.CreateDelivery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            transaction.DeliveryAdapter.SetDeliverySystem(deliverySystem.Object);
            TransactionStatus transactionStatus = await transaction.ActivateTransaction(testUser.Username, "0544444444", WEIGHT1, testStoreAddress, testUserAddress, testUserCreditCard, testStore.GetId(), testStore.name, testStoreCreditCard, 1, shoppingBasket);
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
            HashSet<ProductInCart> product_quantity = new HashSet<ProductInCart>();
            product_quantity.Add(new ProductInCart(product, 10));
            testStore.CancelTransaction(product_quantity);
            Assert.AreEqual(product.Quantity, originalQuantity + 10);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task SucceessPurchaseBasketBecomesEmpty()
        {
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsTrue(!v1.IsErr);
            Assert.IsTrue(testUser.ShoppingCart.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task FailedPurchaseBasketRemainsTheSame()
        {
            await testUser.UpdateProductInShoppingBasket(testStore, product, 5);
            testStore.UpdateProduct(product);
            Mock<ExternalPaymentSystem> paymentSystem = new Mock<ExternalPaymentSystem>();
            paymentSystem.Setup(p => p.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("-1"));
            Transaction transaction = Transaction.Instance;
            transaction.PaymentAdapter.SetPaymentSystem(paymentSystem.Object);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsFalse(!v1.IsErr);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task LastProductTwoCustomersSynchro()
        {
            bool val1 = false;
            bool val2 = false;
            Product oneProduct = new Product(1, 100, 100);
            User secondTestUser = new User("second test user");
            testStore.UpdateProduct(oneProduct);
            await testUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            await secondTestUser.UpdateProductInShoppingBasket(testStore, oneProduct, 1);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            var v2 = await secondTestUser.PurchaseShoppingCart(new CreditCard("1", "1", "1", "1", "1", "1"), "0533333333", new Address("2", "2", "2", "2", "2"));
            val1 = !v1.IsErr;
            val2 = !v2.IsErr;
            Assert.IsTrue(val1 || val2);
            Assert.IsFalse(val1 && val2); //should return false because one of the purchases must fail.

        }

        private bool CheckTotalWeightMoreThan400(ShoppingBasket shoppingBasket)
        {
            double weight = shoppingBasket.GetDictionaryProductQuantity().Aggregate(0.0, (total, next) => total + next.product.Weight * next.quantity);
            return weight > 400;
        }

        private bool CheckTotalWeightNoMoreThan400(ShoppingBasket shoppingBasket)
        {
            double weight = shoppingBasket.GetDictionaryProductQuantity().Aggregate(0.0, (total, next) => total + next.product.Weight * next.quantity);
            return weight <= 400;
        }
        
        [TestCleanup]
        public void DeleteAll()
        {
            Store s = new Store("testStore", testStoreCreditCard, testStoreAddress);
            s.Founder = Founder.makeFounder(new MemberState("userTest"), s);
            testStore = s;
            this.shoppingCart = new ShoppingCart(testUser);
            this.shoppingBasket = new ShoppingBasket(shoppingCart, testStore);
            Transaction.Instance.DeleteAllTests();
        }
    }
}
