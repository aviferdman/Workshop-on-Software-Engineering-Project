
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
using TradingSystem.Business.Market.DiscountPackage;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.Business.Payment;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;

namespace TradingSystemTests.DBTests
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
        private ProductData product1;
        private ShoppingCart shoppingCart;
        private ShoppingBasket shoppingBasket;

        public PurchaseTests()
        {
            
        }

        [TestInitialize]
        public async Task InitializeAsync()
        {
            ProxyMarketContext.Instance.IsDebug = false;
            String guestName = MarketUsers.Instance.AddGuest();
            await UserManagement.Instance.SignUp("founder", "123", null);
            await MarketUsers.Instance.AddMember("founder", "123", guestName);
            guestName = MarketUsers.Instance.AddGuest();
            await UserManagement.Instance.SignUp("testUser", "123", null);
            await MarketUsers.Instance.AddMember("testUser", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            testStore = await MarketStores.Instance.CreateStore("testStore", "founder", card, address);
            product1 = new ProductData("test",QUANTITY1, WEIGHT1, PRICE1,"lala");
            testUser = MarketUsers.Instance.GetUserByUserName("testUser");
            shoppingCart = testUser.ShoppingCart;
            shoppingBasket = await shoppingCart.GetShoppingBasket(testStore);
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, testStore.Id, "founder");
            product = result.Ret;
            testUserAddress = new Address("1", "1", "1", "1", "1");
            testStoreAddress = new Address("2", "2", "2", "2", "2");
            testUserCreditCard = new CreditCard("1", "1", "1", "1", "1", "1");
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahseWithDiscount()
        {
            await MarketUsers.Instance.AddProductToCart(testUser.Username, product.id, 5);
            ConditionDiscount discount = new ConditionDiscount(new DiscountCalculator(return15));
            IRule rule = new Rule(CheckTotalWeightMoreThan400);
            discount.AddRule(rule);
            testStore.AddDiscount(testStore.GetFounder().Username, discount);
            Assert.AreEqual(PRICE1 * 5 * (1 - (DISCOUNT_VALUE / 100)), testUser.ShoppingCart.CalcPaySum());
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsTrue(!v1.IsErr);
        }

        private DiscountOfProducts return15(ShoppingBasket arg)
        {
            var d = new DiscountOfProducts();
            d.AddProduct(product.Id, product.Price - 15);
            d.Discount = 15;
            return d;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task CheckLegalPurcahseUpdatedQuantityAtStore()
        {
            int originQuantity = product.Quantity;
            await MarketUsers.Instance.AddProductToCart(testUser.Username, product.id, 5);
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
            await MarketUsers.Instance.AddProductToCart(testUser.Username, product.id, 5);
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
            await MarketUsers.Instance.AddProductToCart(testUser.Username, product.id, 5);
            Assert.IsFalse(testUser.ShoppingCart.IsEmpty());
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            Assert.IsTrue(!v1.IsErr);
            Assert.IsTrue(testUser.ShoppingCart.IsEmpty());
        }

        /// test for function :<see cref="TradingSystem.Business.Market.User.PurchaseShoppingCart(CreditCard, string, Address)"/>
        [TestMethod]
        public async Task FailedPurchaseBasketRemainsTheSame()
        {
            await MarketUsers.Instance.AddProductToCart(testUser.Username, product.id, 5);
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
            ProductData oneProduct = new ProductData("name22",1, 100, 100, "cat");
            Result<Product> result = await MarketStores.Instance.AddProduct(oneProduct, testStore.Id, "founder");
            await MarketUsers.Instance.AddProductToCart(testUser.Username, result.Ret.id, 1);
            await MarketUsers.Instance.AddProductToCart("founder", result.Ret.id, 1);
            var v1 = await testUser.PurchaseShoppingCart(testUserCreditCard, "0544444444", testUserAddress);
            var v2 = await MarketUsers.Instance.GetUserByUserName("founder").PurchaseShoppingCart(new CreditCard("1", "1", "1", "1", "1", "1"), "0533333333", new Address("2", "2", "2", "2", "2"));
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
            MarketUsers.Instance.tearDown();
            Transaction.Instance.DeleteAllTests();
            MarketStores.Instance.tearDown();
            UserManagement.Instance.tearDown();
        }
    }
}
