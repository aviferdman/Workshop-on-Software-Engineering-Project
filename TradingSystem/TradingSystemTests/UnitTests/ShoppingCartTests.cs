using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.Business.Payment;
using TradingSystem.DAL;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class ShoppingCartTests
    {

        private User testUser;

        public ShoppingCartTests()
        {
            this.testUser = new User("usertest");
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.CheckPolicy())"/>
        [TestMethod]
        public void CheckAllStoresReturnTrue()
        {
            Mock<Store> store1 = new Mock<Store>();
            store1.Setup(s => s.CheckPolicy(It.IsAny<ShoppingBasket>())).Returns(true);
            ShoppingCart shoppingCart = new ShoppingCart(testUser);
            Mock<ShoppingBasket> shoppingBasket1 = new Mock<ShoppingBasket>();
            shoppingBasket1.Setup(basket => basket.GetStore()).Returns(store1.Object);
            shoppingCart.ShoppingBaskets.Add(shoppingBasket1.Object);
            Assert.AreEqual(true, shoppingCart.CheckPolicy());

        }
        
        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.CheckPolicy())"/>
        [TestMethod]
        public void CheckStoresReturnFalse()
        {
            Mock<Store> store1 = new Mock<Store>();
            store1.Setup(s => s.CheckPolicy(It.IsAny<ShoppingBasket>())).Returns(false);
            ShoppingCart shoppingCart = new ShoppingCart(testUser);
            Mock<ShoppingBasket> shoppingBasket1 = new Mock<ShoppingBasket>();
            shoppingBasket1.Setup(basket => basket.GetStore()).Returns(store1.Object);
            shoppingCart.ShoppingBaskets.Add(shoppingBasket1.Object);
            Assert.AreEqual(false, shoppingCart.CheckPolicy());

        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, CreditCard, string, Address, double))"/>
        [TestMethod]
        public async Task CheckPurchaseWithLegalPurchaseStatus()
        {
            ShoppingCart shoppingCart = new ShoppingCart(testUser);
            HashSet<ProductInCart> product_quantity = new HashSet<ProductInCart>();
            Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(new ProductInCart(product1, 1));
            product_quantity.Add(new ProductInCart(product2, 2));
            PurchaseStatus purchaseStatus = new PurchaseStatus(false, new TransactionStatus("Testusername", storeId, null, null, null, true), storeId);

            Mock<Store> store1 = new Mock<Store>();
            store1.Setup(s => s.Purchase(It.IsAny<ShoppingBasket>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<CreditCard>())).Returns(Task.FromResult(purchaseStatus));
            Mock<ShoppingBasket> shoppingBasket1 = new Mock<ShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            shoppingBasket1.Setup(basket => basket.GetStore()).Returns(store1.Object);
            shoppingCart.ShoppingBaskets.Add(shoppingBasket1.Object);
            var v1 = await shoppingCart.Purchase(card, clientPhone, clientAddress);
            Assert.AreEqual(true, v1.Status);
        }

        
        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, CreditCard, string, Address, double))"/>
        [TestMethod]
        public async Task CheckPurchaseDeliveryFailed()
        {
            ShoppingCart shoppingCart = new ShoppingCart(testUser);
            HashSet<ProductInCart> product_quantity = new HashSet<ProductInCart>();
            Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(new ProductInCart(product1, 1));
            product_quantity.Add(new ProductInCart(product2, 2));
            DeliveryStatus deliveryStatus = new DeliveryStatus(Guid.NewGuid().ToString(), testUser.Username, storeId, "storeName", false);
            PaymentStatus paymentStatus = new PaymentStatus(Guid.NewGuid().ToString(), testUser.Username, storeId, true);
            Mock<ShoppingBasket> shoppingBasket1 = new Mock<ShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            TransactionStatus transactionStatus = new TransactionStatus("Testusername", storeId, paymentStatus, deliveryStatus, shoppingBasket1.Object, true);
            PurchaseStatus purchaseStatus = new PurchaseStatus(true, transactionStatus, storeId);

            Mock<Store> store1 = new Mock<Store>();
            store1.Setup(s => s.Purchase(It.IsAny<ShoppingBasket>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<CreditCard>())).Returns(Task.FromResult(purchaseStatus));
            shoppingBasket1.Setup(basket => basket.GetStore()).Returns(store1.Object);
            shoppingCart.ShoppingBaskets.Add(shoppingBasket1.Object);
            var v1 = await shoppingCart.Purchase(card, clientPhone, clientAddress);
            Assert.AreEqual(false, v1.Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, CreditCard, string, Address, double))"/>
        [TestMethod]
        public async Task CheckPurchasePayFailed()
        {
            ShoppingCart shoppingCart = new ShoppingCart(testUser);
            HashSet<ProductInCart> product_quantity = new HashSet<ProductInCart>(); Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(new ProductInCart(product1, 1));
            product_quantity.Add(new ProductInCart(product2, 2));
            DeliveryStatus deliveryStatus = new DeliveryStatus(Guid.NewGuid().ToString(), testUser.Username, storeId, "storeName", true);
            PaymentStatus paymentStatus = new PaymentStatus(Guid.NewGuid().ToString(), testUser.Username, storeId, false);
            Mock<ShoppingBasket> shoppingBasket1 = new Mock<ShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            TransactionStatus transactionStatus = new TransactionStatus("Testusername", storeId, paymentStatus, deliveryStatus, shoppingBasket1.Object, true);
            PurchaseStatus purchaseStatus = new PurchaseStatus(true, transactionStatus, storeId);
            Mock<Store> store1 = new Mock<Store>();
            store1.Setup(s => s.Purchase(It.IsAny<ShoppingBasket>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<CreditCard>())).Returns(Task.FromResult(purchaseStatus));
            shoppingBasket1.Setup(basket => basket.GetStore()).Returns(store1.Object);
            shoppingCart.ShoppingBaskets.Add(shoppingBasket1.Object);
            var v1 = await shoppingCart.Purchase(card, clientPhone, clientAddress);
            Assert.AreEqual(false, v1.Status);

        }
        

        [TestCleanup]
        public void DeleteAll()
        {
            this.testUser = new User("usertest");
            Transaction.Instance.DeleteAllTests();
        }


        //END OF UNIT TESTS

    }
}
