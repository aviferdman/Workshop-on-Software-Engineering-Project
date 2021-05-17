using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.UserBridge;
using AcceptanceTests.AppInterface.Data;
using static TradingSystem.Business.Market.StoreStates.Manager;
using TradingSystem.PublisherComponent;
using TradingSystem.Notifications;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;
using System.Threading.Tasks;

namespace AcceptanceTests.Tests.LiveNotification
{
    public class PurchaseNotificationAT
    {
        IMarketBridge marketBridge = SystemContext.Instance.MarketBridge;
        IUserBridge Bridge = SystemContext.Instance.UserBridge;
        PublisherManagement publisherManagement = PublisherManagement.Instance;
        UserInfo founder = new UserInfo("founder", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        Address a = new Address
        {
            State = "Israel",
            City = "City 3",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        };

        UserInfo buyer1 = new UserInfo("buyer1", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        UserInfo buyer2 = new UserInfo("buyer2", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });
        UserInfo owner1 = new UserInfo("owner1", "123", null, new Address
        {
            State = "Israel",
            City = "City 2",
            Street = "Hello",
            ApartmentNum = "5",
            ZipCode = "55555",
        });

        ProductInfo p = new ProductInfo("potato", 5, 5.0, "veggies", 0.5);
        ProductId? pid;

        ShopId? store;

        [SetUp]
        public void Initialize()
        {
            publisherManagement.DeleteAll();
            Bridge.Connect();
            Bridge.SignUp(buyer2);
            Bridge.Login(buyer2);
            Bridge.Logout();
            Bridge.SignUp(owner1);
            Bridge.Login(owner1);
            Bridge.Logout();
            Bridge.SignUp(founder);
            Bridge.Login(founder);
            store = marketBridge.OpenShop(new ShopInfo
            (
                "whyy",
                new CreditCard
                (
                    cardNumber: "1452369878887888",
                    month: "09",
                    year: "26",
                    holderName: "Nunu Willamp",
                    cvv: "000",
                    holderId: "030301777"
                ),
                new Address
                {
                    State = "Israel",
                    City = "City 2",
                    Street = "Hello",
                    ApartmentNum = "5",
                    ZipCode = "55555",
                })
            );
            pid =marketBridge.AddProductToShop(store.Value, p);
            Bridge.Logout();
            Bridge.SignUp(buyer1);
            Bridge.Login(buyer1);
            double weight = p.Weight;
            string addressSource ="address";
            string addressDest = "address2";
            var packageId = Guid.NewGuid();

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>()
              )).Returns(new Task<string>( () => packageId.ToString()));
            var paymentId = Guid.NewGuid();
            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            _ = paymenySystemMock.Setup(ps => ps.CreatePaymentAsync
              (
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>()
              )).Returns(new Task<string>( () => paymentId.ToString()));

        }

        [Test]
        public async Task CheckValidPurchaseFounderNotified()
        {
            ATSubscriber s = new ATSubscriber("founder");
            publisherManagement.Subscribe("founder", s, EventType.PurchaseEvent);
            marketBridge.AddProductToUserCart(new ProductInCart(pid.Value, 3));
            Assert.IsTrue(await marketBridge.PurchaseShoppingCart(new PurchaseInfo
            (
                "0501345677",
                new CreditCard
                (
                    cardNumber: "5555000011113333",
                    month: "01",
                    year: "23",
                    holderName: "Forsen",
                    cvv: "999",
                    holderId: "302301007"
                ),
                a
            )));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "PurchaseEvent");
        }

        [Test]
        public async Task CheckValidPurchaseAllOwnersNotified()
        {
            ATSubscriber s = new ATSubscriber("founder");
            publisherManagement.Subscribe("founder", s, EventType.PurchaseEvent);
            AppointOwner1();
            ATSubscriber s1 = new ATSubscriber("owner1");
            publisherManagement.Subscribe("owner1", s1, EventType.PurchaseEvent);
            Bridge.Login(buyer1);
            marketBridge.AddProductToUserCart(new ProductInCart(pid.Value, 3));
            Assert.IsTrue(await marketBridge.PurchaseShoppingCart(new PurchaseInfo
            (
                "0501345677",
                new CreditCard
                (
                    cardNumber: "5555000011113333",
                    month: "01",
                    year: "23",
                    holderName: "Forsen",
                    cvv: "999",
                    holderId: "302301007"
                ),
                a
            )));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.AreEqual(s.EventsReceived.Count, 1);
            Assert.AreEqual(s.EventsReceived.Dequeue().EventProviderName, "PurchaseEvent");
            Assert.AreEqual(s1.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s1.EventsReceived.Count, 1);
            Assert.AreEqual(s1.EventsReceived.Dequeue().EventProviderName, "PurchaseEvent");

        }

        private void AppointOwner1()
        {
            Bridge.Logout();
            Bridge.Login(founder);
            marketBridge.MakeOwner("owner1", store.Value, "founder");
            Bridge.Logout();
            Bridge.Login(owner1);
            Bridge.Logout();
        }

        [Test]
        public async Task CheckInvalidPurchaseAllOwnerNotNotified()
        {
            ATSubscriber s = new ATSubscriber("founder");
            publisherManagement.Subscribe("founder", s, EventType.PurchaseEvent);
            AppointOwner1();
            ATSubscriber s1 = new ATSubscriber("owner1");
            publisherManagement.Subscribe("owner1", s1, EventType.PurchaseEvent);
            Bridge.Login(buyer1);
            marketBridge.AddProductToUserCart(new ProductInCart(pid.Value, 3));
            Bridge.Logout();
            Bridge.Login(founder);
            p.Quantity = 2;
            marketBridge.EditProductInShop(store.Value, pid.Value, p);
            Bridge.Logout();
            Bridge.Login(buyer1);
            Assert.IsFalse(await marketBridge.PurchaseShoppingCart(new PurchaseInfo
            (
                "0501345677",
                new CreditCard
                (
                    cardNumber: "5555000011113333",
                    month: "01",
                    year: "23",
                    holderName: "Forsen",
                    cvv: "999",
                    holderId: "302301007"
                ),
                a
            )));
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(founder);
            Assert.AreEqual(s.EventsReceived.Count, 0);
            Assert.AreEqual(s1.EventsReceived.Count, 0);
            Bridge.Logout();
            Bridge.Login(owner1);
            Assert.AreEqual(s1.EventsReceived.Count, 0);
            p.Quantity = 5;
        }

        [TearDown]
        public void DeleteAll()
        {
            Bridge.Logout();
            Bridge.Disconnect();
            Bridge.tearDown();
            marketBridge.tearDown();
        }
    }
}
