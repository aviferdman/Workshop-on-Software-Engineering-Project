using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using Moq;
using Moq.Language.Flow;

using NUnit.Framework;

using TradingSystem.Business.Delivery;
using TradingSystem.Business.Payment;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance tests for:
    /// Use case 11: Buy products in shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/77
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_PurchaseCart : MarketTestBase
    {
        private static readonly object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User_Buyer,
                User_Buyer2,
                SharedTestsData.User_Buyer3,
                SharedTestsData.User_Other,
                new ShopImage
                (
                    User_ShopOwner1,
                    Shop1,
                    new ProductIdentifiable[]
                    {
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "computer keyboard",
                            quantity: 140,
                            price: 90,
                            category: "computer peripherals",
                            weight: 0.5
                        )),
                        new ProductIdentifiable(new ProductInfo
                        (
                            name: "nvidia gtx 3090",
                            quantity: 5,
                            price: 2000,
                            category: "graphic cards",
                            weight: 5
                        )),
                    }
                ),
            },
        };

        public UseCase_PurchaseCart
        (
            SystemContext systemContext,
            BuyerUserInfo buyerUser,
            BuyerUserInfo buyerUser2,
            BuyerUserInfo buyerUser_empty,
            BuyerUserInfo competitorUser,
            ShopImage shopImage
        ) : base(systemContext)
        {
            ShopImage = shopImage;
            BuyerUser = buyerUser;
            BuyerUser2 = buyerUser2;
            BuyerUser_Empty = buyerUser_empty;
            CompetitorUser = competitorUser;
        }

        public ShopImage ShopImage { get; }
        public BuyerUserInfo BuyerUser { get; }
        public BuyerUserInfo BuyerUser2 { get; }
        public BuyerUserInfo BuyerUser_Empty { get; }
        public BuyerUserInfo CompetitorUser { get; }

        private Mock<ExternalDeliverySystem> Mock_Delivery { get; set; }
        private string TicketId_Delivery { get; set; }
        private Mock<ExternalPaymentSystem> Mock_Payment { get; set; }
        private string TicketId_Payment { get; set; }
        private string TicketId_Payment_Cancel { get; set; }

        private UseCase_AddProductToShop useCase_addProductToShop;
        private UseCase_AddProductToCart_TestLogic useCase_AddProductToCart_TestLogic_1;
        private UseCase_AddProductToCart_TestLogic useCase_AddProductToCart_TestLogic_2;
        private UseCase_AddProductToCart_TestLogic useCase_AddProductToCart_TestLogic_competitor;
        private UseCase_Login? useCase_login;

        public override void Setup()
        {
            base.Setup();
            useCase_addProductToShop = new UseCase_AddProductToShop(SystemContext, ShopImage);
            useCase_addProductToShop.Setup();
            useCase_addProductToShop.Success_Normal_CheckStoreProducts();
            useCase_AddProductToCart_TestLogic_1 = AddProductsToCart(BuyerUser, GetTestProductEnumerable(60));
            useCase_AddProductToCart_TestLogic_2 = AddProductsToCart(BuyerUser2, GetTestProductEnumerable(60));
            useCase_AddProductToCart_TestLogic_competitor = AddProductsToCart(CompetitorUser, GetTestProductEnumerable(81));
            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
        }

        public override void Teardown()
        {
            useCase_AddProductToCart_TestLogic_competitor?.Teardown();
            useCase_AddProductToCart_TestLogic_2?.Teardown();
            useCase_AddProductToCart_TestLogic_1?.Teardown();
            useCase_addProductToShop?.Teardown();
            useCase_login?.Teardown();
            MarketBridge.DisableExternalTransactionMocks();
            base.Teardown();
        }

        private UseCase_AddProductToCart_TestLogic AddProductsToCart(UserInfo user, IEnumerable<ProductForCart> products)
        {
            var useCase_addProductToCart_testLogic = new UseCase_AddProductToCart_TestLogic(SystemContext, user);
            useCase_addProductToCart_testLogic.Setup();
            IEnumerable<ProductInCart> products_add = ProductForCart.ToProductInCart(products);
            useCase_addProductToCart_testLogic.Success_Normal_CheckCartItems(products_add, products_add);
            return useCase_addProductToCart_testLogic;
        }

        private IEnumerable<ProductForCart> GetTestProductEnumerable(int qunatity)
        {
            return new ProductForCart[] { new ProductForCart(ShopImage.ShopProducts[0], qunatity), };
        }

        private void SetupMock_Delivery_Success(BuyerUserInfo buyerUser)
        {
            Mock_Delivery = new Mock<ExternalDeliverySystem>();
            TicketId_Delivery = GenerateTicket();
            _ = SetupMock_Delivery(Mock_Delivery, buyerUser).Returns(Task.FromResult(TicketId_Delivery));
        }
        private void SetupMock_Delivery_Fails(BuyerUserInfo buyerUser)
        {
            Mock_Delivery = new Mock<ExternalDeliverySystem>();
            _ = SetupMock_Delivery(Mock_Delivery, buyerUser).Returns(Task.FromResult("-1"));
        }
        private ISetup<ExternalDeliverySystem, Task<string>> SetupMock_Delivery(Mock<ExternalDeliverySystem> mock, BuyerUserInfo buyerUser)
        {
            return mock.Setup(ds => ds.CreateDelivery
            (
                It.Is<string>(x => x == buyerUser.Username),
                It.Is<string>(x => x == $"{buyerUser.Address.Street} {buyerUser.Address.ApartmentNum}"),
                It.Is<string>(x => x == buyerUser.Address.City),
                It.Is<string>(x => x == buyerUser.Address.State),
                It.Is<string>(x => x == buyerUser.Address.ZipCode)
            ));
        }
        private void SetupMock_Delivery_NeverCalled(string message)
        {
            Mock_Delivery = new Mock<ExternalDeliverySystem>();
            Mock_Delivery.Verify(ds => ds.CreateDelivery
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), message);
        }

        private void SetupMock_Payment_Success(BuyerUserInfo buyerUser)
        {
            Mock_Payment = new Mock<ExternalPaymentSystem>();
            TicketId_Payment = GenerateTicket();
            _ = SetupMock_Payment(Mock_Payment, buyerUser).Returns(Task.FromResult(TicketId_Payment));
        }
        private void SetupMock_Payment_Fails(BuyerUserInfo buyerUser)
        {
            Mock_Payment = new Mock<ExternalPaymentSystem>();
            _ = SetupMock_Payment(Mock_Payment, buyerUser).Returns(Task.FromResult("-1"));
        }
        private ISetup<ExternalPaymentSystem, Task<string>> SetupMock_Payment(Mock<ExternalPaymentSystem> mock, BuyerUserInfo buyerUser)
        {
            return mock.Setup(ps => ps.CreatePaymentAsync
            (
                It.Is<string>(x => x == buyerUser.CreditCard.CardNumber),
                It.Is<string>(x => x == buyerUser.CreditCard.Month),
                It.Is<string>(x => x == buyerUser.CreditCard.Year),
                It.Is<string>(x => x == buyerUser.CreditCard.HolderName),
                It.Is<string>(x => x == buyerUser.CreditCard.Cvv),
                It.Is<string>(x => x == buyerUser.CreditCard.HolderId)
            ));
        }
        private void SetupMock_Payment_NeverCalled(string message)
        {
            Mock_Payment = new Mock<ExternalPaymentSystem>();
            Mock_Payment.Verify(ps => ps.CreatePaymentAsync
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), message);
        }

        private string GenerateTicket()
        {
            return Guid.NewGuid().ToString();
        }

        private void IntasllMocks()
        {
            MarketBridge.SetExternalTransactionMocks(Mock_Delivery, Mock_Payment);
        }

        private UseCase_Login Prepare(BuyerUserInfo buyerUser, Action<BuyerUserInfo> setupMocks)
        {
            UseCase_Login useCase_Login = LoginAssure(buyerUser);
            setupMocks(buyerUser);
            IntasllMocks();
            return useCase_Login;
        }

        private void AssertHistory_Empty()
        {
            AssertHistory(Enumerable.Empty<PurchaseHistoryRecord>());
        }

        private void AssertHistory(IEnumerable<PurchaseHistoryRecord> expectedHistoryRecords)
        {
            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            new Assert_SetEquals<string, PurchaseHistoryRecord>
            (
                expectedHistoryRecords,
                x => x.PaymentId,
                (record_expected, record_actual) =>
                {
                    //new Assert_SetEquals<ProductId, ProductInCart>
                   // (
                      //  record_expected.Products,
                    //    x => x.ProductId
                   // ).AssertEquals(record_actual.Products);
                    Assert.AreEqual(record_expected.PaymentStatus, record_actual.PaymentStatus, "Expected a success payment status.");
                    Assert.AreEqual(record_expected.DeliveryStatus, record_actual.DeliveryStatus, "Expected a success delivery status.");
                    Assert.AreEqual(record_expected.PaymentId, record_actual.PaymentId, "The payment id is different than expected.");
                    Assert.AreEqual(record_expected.DeliveryPackageId, record_actual.DeliveryPackageId, "The delivery package id is different than expected.");
                    return true;
                }
            ).AssertEquals(purchaseHistory);
        }

       // [TestCase]
        public async Task Success_Normal()
        {
            await Success_Normal(BuyerUser, useCase_AddProductToCart_TestLogic_1.Products!);
        }

        private async Task Success_Normal(BuyerUserInfo buyerUser, IEnumerable<ProductInCart> expectedProductsCartHistory)
        {
            _ = Prepare(buyerUser, buyerUser =>
            {
                SetupMock_Delivery_Success(buyerUser);
                SetupMock_Payment_Success(buyerUser);
            });

            Assert.IsTrue(await MarketBridge.PurchaseShoppingCart(buyerUser));
            AssertHistory(new PurchaseHistoryRecord[]
            {
                new PurchaseHistoryRecord(expectedProductsCartHistory, TicketId_Delivery, true, TicketId_Payment, true),
            });
        }

        [TestCase]
        public async Task Failure_ProductsUnavailable()
        {
            // Another user buys enough units so the primary user don't enough left
            await Success_Normal(CompetitorUser, useCase_AddProductToCart_TestLogic_competitor.Products!);

            // log in again to the primary user and re-setup the mocks
            _ = Prepare(BuyerUser2, buyerUser =>
            {
                SetupMock_Delivery_NeverCalled("Delivery have been called despite the products unavailablity in the shop.");
                SetupMock_Payment_NeverCalled("Payment have been called despite the products unavailablity in the shop.");
            });

            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(BuyerUser2));
            AssertHistory_Empty();
        }

        [TestCase]
        public async Task Failure_PaymentFailed()
        {
            _ = Prepare(BuyerUser, buyerUser =>
            {
                SetupMock_Delivery_NeverCalled("Delivery have been called despite payment failure.");
                SetupMock_Payment_Fails(buyerUser);
            });

            Assert.IsFalse(await MarketBridge .PurchaseShoppingCart(BuyerUser));
            AssertHistory_Empty();
        }

        [TestCase]
        public async Task Failure_DeliveryFailed()
        {
            _ = Prepare(BuyerUser, buyerUser =>
            {
                SetupMock_Delivery_Fails(buyerUser);
                SetupMock_Payment_Success(buyerUser);

                // make sure cancel payment is called
                TicketId_Payment_Cancel = GenerateTicket();
                _ = Mock_Payment
                    .Setup(ps => ps.CancelPayment(It.Is<string>(x => x.Equals(TicketId_Payment))))
                    .Returns(Task.FromResult(TicketId_Payment_Cancel));
            });

            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(BuyerUser));
            AssertHistory_Empty();
        }

        [TestCase]
        public async Task Failure_EmptyCart()
        {
            useCase_login = Prepare(BuyerUser_Empty, buyerUser =>
            {
                SetupMock_Delivery_NeverCalled("Delivery have been called despite no products in cart.");
                SetupMock_Payment_NeverCalled("Payment have been called despite no products in cart.");
            });

            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(BuyerUser));
            AssertHistory_Empty();
        }
    }
}
