using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;

namespace TradingSystemTests.MarketTests
{
    [TestClass]
    public class ShoppingCartTests
    {

        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.CheckPolicy())"/>
        [TestMethod]
        public void CheckAllStoresReturnTrue()
        {
            Mock<IStore> store1 = new Mock<IStore>();
            store1.Setup(s => s.CheckPolicy(It.IsAny<IShoppingBasket>())).Returns(true);
            ShoppingCart shoppingCart = new ShoppingCart();
            Mock<IShoppingBasket> shoppingBasket1 = new Mock<IShoppingBasket>();
            Mock<IShoppingBasket> shoppingBasket2 = new Mock<IShoppingBasket>();
            shoppingCart.Store_shoppingBasket.Add(store1.Object, shoppingBasket1.Object);
            Assert.AreEqual(true, shoppingCart.CheckPolicy());

        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.CheckPolicy())"/>
        [TestMethod]
        public void CheckStoresReturnFalse()
        {
            Mock<IStore> store1 = new Mock<IStore>();
            store1.Setup(s => s.CheckPolicy(It.IsAny<IShoppingBasket>())).Returns(false);
            ShoppingCart shoppingCart = new ShoppingCart();
            Mock<IShoppingBasket> shoppingBasket1 = new Mock<IShoppingBasket>();
            Mock<IShoppingBasket> shoppingBasket2 = new Mock<IShoppingBasket>();
            shoppingCart.Store_shoppingBasket.Add(store1.Object, shoppingBasket1.Object);
            Assert.AreEqual(false, shoppingCart.CheckPolicy());

        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, BankAccount, string, Address, double))"/>
        [TestMethod]
        public void CheckPurchaseWithLegalPurchaseStatus()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            string clienId = "usertest";
            Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(200, 200);
            double paySum = 1;
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            PurchaseStatus purchaseStatus = new PurchaseStatus(false, new TransactionStatus(null, null, null, true), storeId);

            Mock<IStore> store1 = new Mock<IStore>();
            store1.Setup(s => s.Purchase(It.IsAny<IShoppingBasket>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<BankAccount>(), It.IsAny<double>())).Returns(purchaseStatus);
            Mock<IShoppingBasket> shoppingBasket1 = new Mock<IShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            shoppingCart.Store_shoppingBasket.Add(store1.Object, shoppingBasket1.Object);
            Assert.AreEqual(true, shoppingCart.Purchase(clienId, bankAccount, clientPhone, clientAddress, paySum).Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, BankAccount, string, Address, double))"/>
        [TestMethod]
        public void CheckPurchaseDeliveryFailed()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            string clientId = "usertest";
            Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(200, 200);
            double paySum = 1;
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            DeliveryStatus deliveryStatus = new DeliveryStatus(Guid.NewGuid(), clientId, storeId, false);
            PaymentStatus paymentStatus = new PaymentStatus(Guid.NewGuid(), clientId, storeId, true);
            Mock<IShoppingBasket> shoppingBasket1 = new Mock<IShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            TransactionStatus transactionStatus = new TransactionStatus(paymentStatus, deliveryStatus, shoppingBasket1.Object, true);
            PurchaseStatus purchaseStatus = new PurchaseStatus(true, transactionStatus, storeId);

            Mock<IStore> store1 = new Mock<IStore>();
            store1.Setup(s => s.Purchase(It.IsAny<IShoppingBasket>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<BankAccount>(), It.IsAny<double>())).Returns(purchaseStatus);

            shoppingCart.Store_shoppingBasket.Add(store1.Object, shoppingBasket1.Object);
            Assert.AreEqual(false, shoppingCart.Purchase(clientId, bankAccount, clientPhone, clientAddress, paySum).Status);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.ShoppingCart.Purchase(Guid, BankAccount, string, Address, double))"/>
        [TestMethod]
        public void CheckPurchasePayFailed()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            Dictionary<Product, int> product_quantity = new Dictionary<Product, int>();
            string clientId = "usertest";
            Guid storeId = Guid.NewGuid();
            string clientPhone = "0544444444";
            Address clientAddress = new Address("1", "1", "1", "1");
            BankAccount bankAccount = new BankAccount(200, 200);
            double paySum = 1;
            Product product1 = new Product(10, 10, 10);
            Product product2 = new Product(20, 20, 20);
            product_quantity.Add(product1, 1);
            product_quantity.Add(product2, 2);
            DeliveryStatus deliveryStatus = new DeliveryStatus(Guid.NewGuid(), clientId, storeId, true);
            PaymentStatus paymentStatus = new PaymentStatus(Guid.NewGuid(), clientId, storeId, false);
            Mock<IShoppingBasket> shoppingBasket1 = new Mock<IShoppingBasket>();
            shoppingBasket1.Setup(sb => sb.GetDictionaryProductQuantity()).Returns(product_quantity);
            TransactionStatus transactionStatus = new TransactionStatus(paymentStatus, deliveryStatus, shoppingBasket1.Object, true);
            PurchaseStatus purchaseStatus = new PurchaseStatus(true, transactionStatus, storeId);
            Mock<IStore> store1 = new Mock<IStore>();
            store1.Setup(s => s.Purchase(It.IsAny<IShoppingBasket>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Address>(), It.IsAny<BankAccount>(), It.IsAny<double>())).Returns(purchaseStatus);

            shoppingCart.Store_shoppingBasket.Add(store1.Object, shoppingBasket1.Object);
            Assert.AreEqual(false, shoppingCart.Purchase(clientId, bankAccount, clientPhone, clientAddress, paySum).Status);

        }
        


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
        }


        //END OF UNIT TESTS

    }
}
