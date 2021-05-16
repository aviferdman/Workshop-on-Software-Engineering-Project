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

        [TestCase]
        public async Task Success_Normal()
        {
            await Success_Normal(BuyerUser, useCase_AddProductToCart_TestLogic_1.Products!);
        }

        private async Task Success_Normal(BuyerUserInfo buyerUser, IEnumerable<ProductInCart> expectedProductsCartHistory)
        {
            _ = Login(buyerUser);

            var packageId = Guid.NewGuid();
            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == buyerUser.Username),
                  It.Is<string>(x => x == $"{buyerUser.Address.Street} {buyerUser.Address.ApartmentNum}"),
                  It.Is<string>(x => x == buyerUser.Address.City),
                  It.Is<string>(x => x == buyerUser.Address.State),
                  It.Is<string>(x => x == buyerUser.Address.ZipCode)
              )).Returns(Task.FromResult(packageId.ToString()));

            var paymentId = Guid.NewGuid();
            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            _ = paymenySystemMock.Setup(ps => ps.CreatePaymentAsync
              (
                  It.Is<string>(x => x == buyerUser.CreditCard.CardNumber),
                  It.Is<string>(x => x == buyerUser.CreditCard.Month),
                  It.Is<string>(x => x == buyerUser.CreditCard.Year),
                  It.Is<string>(x => x == buyerUser.CreditCard.HolderName),
                  It.Is<string>(x => x == buyerUser.CreditCard.Cvv),
                  It.Is<string>(x => x == buyerUser.CreditCard.HolderId)
              )).Returns(Task.FromResult(paymentId.ToString()));

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsTrue(await MarketBridge.PurchaseShoppingCart(new PurchaseInfo(buyerUser.PhoneNumber, buyerUser.CreditCard, buyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsTrue(purchaseHistory.Count() == 1, "Expected exactly 1 purchase in history.");

            PurchaseHistoryRecord purchase = purchaseHistory.First();
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                expectedProductsCartHistory,
                x => x.ProductId
            ).AssertEquals(purchase);

            Assert.IsTrue(purchase.PaymentStatus, "Expected a success payment status.");
            Assert.IsTrue(purchase.DeliveryStatus, "Expected a success delivery status.");
            Assert.AreEqual(paymentId.ToString(), purchase.PaymentId, "The payment id is different than expected.");
            Assert.AreEqual(packageId.ToString(), purchase.DeliveryPackageId, "The delivery package id is different than expected.");
        }

        [TestCase]
        public async Task Failure_ProductsUnavailable()
        {
            // Another user buys enough units so the primary user don't enough left
            await Success_Normal(CompetitorUser, useCase_AddProductToCart_TestLogic_competitor.Products!);

            // log in again to the primary user
            LoginAssure(BuyerUser);

            // now the primary user tries to buy
            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            deliverySytemMock.Verify(ds => ds.CreateDelivery
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Delivery have been called despite the products unavailablity in the shop.");

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Verify(ps => ps.CreatePaymentAsync
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Payment have been called despite the products unavailablity in the shop.");

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser2.PhoneNumber, BuyerUser2.CreditCard, BuyerUser2.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public async Task Failure_PaymentFailed()
        {
            _ = Login(BuyerUser);

            var packageId = Guid.NewGuid();
            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == BuyerUser.Username),
                  It.Is<string>(x => x == $"{BuyerUser.Address.Street} {BuyerUser.Address.ApartmentNum}"),
                  It.Is<string>(x => x == BuyerUser.Address.City),
                  It.Is<string>(x => x == BuyerUser.Address.State),
                  It.Is<string>(x => x == BuyerUser.Address.ZipCode)
              )).Returns(Task.FromResult(packageId.ToString()));

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Setup(ps => ps.CreatePaymentAsync
            (
                  It.Is<string>(x => x == BuyerUser.CreditCard.CardNumber),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Month),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Year),
                  It.Is<string>(x => x == BuyerUser.CreditCard.HolderName),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Cvv),
                  It.Is<string>(x => x == BuyerUser.CreditCard.HolderId)
            )).Returns(Task.FromResult("-1"));

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(await MarketBridge .PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.CreditCard, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public async Task Failure_DeliveryFailed()
        {
            _ = Login(BuyerUser);

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == BuyerUser.Username),
                  It.Is<string>(x => x == $"{BuyerUser.Address.Street} {BuyerUser.Address.ApartmentNum}"),
                  It.Is<string>(x => x == BuyerUser.Address.City),
                  It.Is<string>(x => x == BuyerUser.Address.State),
                  It.Is<string>(x => x == BuyerUser.Address.ZipCode)
              )).Returns(Task.FromResult("-1"));

            var paymentId = Guid.NewGuid();
            var refundPaymentId = Guid.NewGuid();
            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            _ = paymenySystemMock.Setup(ps => ps.CreatePaymentAsync
            (
                  It.Is<string>(x => x == BuyerUser.CreditCard.CardNumber),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Month),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Year),
                  It.Is<string>(x => x == BuyerUser.CreditCard.HolderName),
                  It.Is<string>(x => x == BuyerUser.CreditCard.Cvv),
                  It.Is<string>(x => x == BuyerUser.CreditCard.HolderId)
            )).Returns(Task.FromResult(paymentId.ToString()));
            _ = paymenySystemMock.Setup(ps => ps
                .CancelPayment(It.Is<string>(x => x.Equals(paymentId.ToString()))))
                .Returns(Task.FromResult(refundPaymentId.ToString()));

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.CreditCard, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public async Task Failure_EmptyCart()
        {
            useCase_login = Login(BuyerUser_Empty);

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            deliverySytemMock.Verify(ds => ds.CreateDelivery
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Delivery have been called despite no products in cart.");

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Verify(ps => ps.CreatePaymentAsync
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Payment have been called despite no products in cart.");

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(await MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.CreditCard, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }
    }
}
