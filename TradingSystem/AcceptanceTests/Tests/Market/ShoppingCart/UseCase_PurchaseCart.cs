using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private UseCase_AddProductToCart? useCase_addProduct;
        private UseCase_AddProductToCart_TestLogic useCase_addProductOther;
        private UseCase_Login? useCase_login;

        public UseCase_PurchaseCart
        (
            SystemContext systemContext,
            BuyerUserInfo buyerUser,
            BuyerUserInfo buyerUser2,
            BuyerUserInfo competitorUser,
            ShopImage shopImage
        ) : base(systemContext)
        {
            ShopImage = shopImage;
            BuyerUser = buyerUser;
            BuyerUser2 = buyerUser2;
            CompetitorUser = competitorUser;
        }

        public ShopImage ShopImage { get; }
        public BuyerUserInfo BuyerUser { get; }
        public BuyerUserInfo BuyerUser2 { get; }
        public BuyerUserInfo CompetitorUser { get; }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            useCase_addProductOther?.Teardown();
            useCase_addProduct?.Teardown();
            useCase_login?.Teardown();
            MarketBridge.DisableExternalTransactionMocks();
            base.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            useCase_addProduct = new UseCase_AddProductToCart
            (
                SystemContext,
                BuyerUser,
                ShopImage,
                shopImage => new ProductForCart[]
                {
                    new ProductForCart(shopImage.ShopProducts[0], 60),
                }
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_NoBasket();

            Success_Normal(BuyerUser, useCase_addProduct.ProductsAdd);
        }

        private void Success_Normal(BuyerUserInfo buyerUser, IEnumerable<ProductForCart> products)
        {
            double weight = products.Select(x => x.ProductIdentifiable.ProductInfo.Weight * x.CartQuantity).Sum();
            string addressSource = ShopImage.ShopInfo.Address.ToString();
            string addressDest = buyerUser.Address.ToString();
            var packageId = Guid.NewGuid();

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == buyerUser.Username),
                  It.Is<string>(x => x == buyerUser.PhoneNumber),
                  It.Is<double>(x => x == weight),
                  It.Is<string>(x => x == addressSource),
                  It.Is<string>(x => x == addressDest)
              )).Returns(packageId);

            double price = products.Select(x => x.ProductIdentifiable.ProductInfo.Price * x.CartQuantity).Sum();
            var paymentId = Guid.NewGuid();

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            _ = paymenySystemMock.Setup(ps => ps.CreatePayment
              (
                  It.Is<string>(x => x == buyerUser.Username),
                  It.Is<string>(x => x == buyerUser.BankAccount.ToString()),
                  It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.AccountNumber),
                  It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.Branch),
                  It.Is<double>(x => x == price)
              )).Returns(paymentId);

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsTrue(MarketBridge.PurchaseShoppingCart(new PurchaseInfo(buyerUser.PhoneNumber, buyerUser.BankAccount, buyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsTrue(purchaseHistory.Count() == 1, "Expected exactly 1 purchase in history.");

            PurchaseHistoryRecord purchase = purchaseHistory.First();
            IEnumerable<ProductInCart> productsCartHistory = ProductForCart.ToProductInCart(products);
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                productsCartHistory,
                x => x.ProductId
            ).AssertEquals(purchase);

            Assert.IsTrue(purchase.PaymentStatus, "Expected a success payment status.");
            Assert.IsTrue(purchase.DeliveryStatus, "Expected a success delivery status.");
            Assert.AreEqual(paymentId, purchase.PaymentId, "The payment id is different than expected.");
            Assert.AreEqual(packageId, purchase.DeliveryPackageId, "The delivery package id is different than expected.");
        }

        [TestCase]
        public void Failure_ProductsUnavailable()
        {
            useCase_addProduct = new UseCase_AddProductToCart
            (
                SystemContext,
                BuyerUser2,
                ShopImage,
                shopImage => new ProductForCart[]
                {
                    new ProductForCart(shopImage.ShopProducts[0], 81),
                }
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_NoBasket();

            // Another user buys enough units so the primary user don't enough left
            useCase_addProductOther = new UseCase_AddProductToCart_TestLogic(SystemContext, CompetitorUser);
            useCase_addProductOther.Setup();
            IEnumerable<ProductForCart> competitorCartProducts = new ProductForCart[]
            {
                new ProductForCart(ShopImage.ShopProducts[0], 60),
            };
            IEnumerable<ProductInCart> competitorCartProducts_add = ProductForCart.ToProductInCart(competitorCartProducts);
            useCase_addProductOther.Success_Normal_CheckCartItems(competitorCartProducts_add, competitorCartProducts_add);
            Success_Normal(CompetitorUser, competitorCartProducts);

            // log in again to the primary user
            var useCase_login = new UseCase_Login(SystemContext, BuyerUser);
            useCase_login.Setup();
            useCase_login.Success_Assure();

            // now the primary user tries to buy
            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            deliverySytemMock.Verify(ds => ds.CreateDelivery
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Delivery have been called despite the products unavailablity in the shop.");

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Verify(ps => ps.CreatePayment
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<double>()
            ), Times.Never(), "Payment have been called despite the products unavailablity in the shop.");

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser2.PhoneNumber, BuyerUser2.BankAccount, BuyerUser2.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public void Failure_PaymentFailed()
        {
            useCase_addProduct = new UseCase_AddProductToCart
            (
                SystemContext,
                BuyerUser,
                ShopImage,
                shopImage => new ProductForCart[]
                {
                    new ProductForCart(shopImage.ShopProducts[0], 60),
                }
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_NoBasket();

            double weight = useCase_addProduct.ProductsAdd.Select(x => x.ProductIdentifiable.ProductInfo.Weight * x.CartQuantity).Sum();
            string addressSource = ShopImage.ShopInfo.Address.ToString();
            string addressDest = BuyerUser.Address.ToString();
            var packageId = Guid.NewGuid();

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == BuyerUser.Username),
                  It.Is<string>(x => x == BuyerUser.PhoneNumber),
                  It.Is<double>(x => x == weight),
                  It.Is<string>(x => x == addressSource),
                  It.Is<string>(x => x == addressDest)
              )).Returns(packageId);

            double price = useCase_addProduct.ProductsAdd.Select(x => x.ProductIdentifiable.ProductInfo.Price * x.CartQuantity).Sum();

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Setup(ps => ps.CreatePayment
            (
                It.Is<string>(x => x == BuyerUser.Username),
                It.Is<string>(x => x == BuyerUser.BankAccount.ToString()),
                It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.AccountNumber),
                It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.Branch),
                It.Is<double>(x => x == price)
            )).Returns(Guid.Empty);

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.BankAccount, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public void Failure_DeliveryFailed()
        {
            useCase_addProduct = new UseCase_AddProductToCart
            (
                SystemContext,
                BuyerUser,
                ShopImage,
                shopImage => new ProductForCart[]
                {
                    new ProductForCart(shopImage.ShopProducts[0], 60),
                }
            );
            useCase_addProduct.Setup();
            useCase_addProduct.Success_NoBasket();

            double weight = useCase_addProduct.ProductsAdd.Select(x => x.ProductIdentifiable.ProductInfo.Weight * x.CartQuantity).Sum();
            string addressSource = ShopImage.ShopInfo.Address.ToString();
            string addressDest = BuyerUser.Address.ToString();

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            _ = deliverySytemMock.Setup(ds => ds.CreateDelivery
              (
                  It.Is<string>(x => x == BuyerUser.Username),
                  It.Is<string>(x => x == BuyerUser.PhoneNumber),
                  It.Is<double>(x => x == weight),
                  It.Is<string>(x => x == addressSource),
                  It.Is<string>(x => x == addressDest)
              )).Returns(Guid.Empty);

            double price = useCase_addProduct.ProductsAdd.Select(x => x.ProductIdentifiable.ProductInfo.Price * x.CartQuantity).Sum();
            var paymentId = Guid.NewGuid();
            var refundPaymentId = Guid.NewGuid();

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            _ = paymenySystemMock.Setup(ps => ps.CreatePayment
            (
                It.Is<string>(x => x == BuyerUser.Username),
                It.Is<string>(x => x == BuyerUser.BankAccount.ToString()),
                It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.AccountNumber),
                It.Is<int>(x => x == ShopImage.ShopInfo.BankAccount.Branch),
                It.Is<double>(x => x == price)
            )).Returns(paymentId);
            _ = paymenySystemMock.Setup(ps => ps
                .CancelPayment(It.Is<Guid>(x => x == paymentId)))
                .Returns(refundPaymentId);

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.BankAccount, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }

        [TestCase]
        public void Failure_EmptyCart()
        {
            useCase_login = new UseCase_Login(SystemContext, BuyerUser);
            useCase_login.Setup();
            useCase_login.Success_Normal();

            var deliverySytemMock = new Mock<ExternalDeliverySystem>();
            deliverySytemMock.Verify(ds => ds.CreateDelivery
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never(), "Delivery have been called despite no products in cart.");

            var paymenySystemMock = new Mock<ExternalPaymentSystem>();
            paymenySystemMock.Verify(ps => ps.CreatePayment
            (
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<double>()
            ), Times.Never(), "Payment have been called despite no products in cart.");

            MarketBridge.SetExternalTransactionMocks(deliverySytemMock, paymenySystemMock);
            Assert.IsFalse(MarketBridge.PurchaseShoppingCart(new PurchaseInfo(BuyerUser.PhoneNumber, BuyerUser.BankAccount, BuyerUser.Address)));

            PurchaseHistory? purchaseHistory = MarketBridge.GetUserPurchaseHistory();
            Assert.IsNotNull(purchaseHistory);
            Assert.IsFalse(purchaseHistory.Any(), "Expected no purchases in history.");
        }
    }
}
