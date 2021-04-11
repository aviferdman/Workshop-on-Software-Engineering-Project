
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 5: Add product to shopping basket
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/53
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_AddProductToCart : MarketMemberTestBase
    {
        private static object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User_Buyer,
                User_ShopOwner1,
                SHOP_NAME,
                new ProductInfo("speakers", 30, 90),
                60
            },
        };

        public UserInfo ShopOwnerUser { get; }
        public string ShopName { get; }
        public ProductInfo ProductInfo { get; }
        public int Quantity { get; }

        public UseCase_AddProductToCart(
            SystemContext systemContext,
            UserInfo buyerUser,
            UserInfo shopOwnerUser,
            string shopName,
            ProductInfo productInfo,
            int quantity
        ) : base(systemContext, buyerUser)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopName = shopName;
            ProductInfo = productInfo;
            Quantity = quantity;
        }

        private UseCase_AddProductToShop useCase_addProduct;
        private UseCase_Login useCase_login_buyer;
        public ProductId ProductId { get; private set; }

        private UseCase_AddProductToCart_TestLogic testLogic;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            useCase_addProduct = new UseCase_AddProductToShop(SystemContext, ShopOwnerUser, ShopName);
            useCase_addProduct.Setup();
            ProductId = useCase_addProduct.Success_Normal(ProductInfo);

            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            useCase_login_buyer = new UseCase_Login(SystemContext, UserInfo);
            useCase_login_buyer.Setup();
            useCase_login_buyer.Success_Normal();

            testLogic = new UseCase_AddProductToCart_TestLogic(SystemContext);
        }

        [TearDown]
        public override void Teardown()
        {
            _ = Bridge.RemoveProductFromUserCart(ProductId);
            useCase_login_buyer?.TearDown();
            bool loggedInToShopOwner = SystemContext.UserBridge.Login(ShopOwnerUser);
            if (loggedInToShopOwner)
            {
                useCase_addProduct?.Teardown();
            }
        }

        [TestCase]
        public void Success_Normal()
        {
            testLogic.Success_Normal(new ProductInCart(ProductId, Quantity));
            new Assert_SetEquals<ProductId, ProductInCart>(
                "Add product to cart - success",
                new ProductInCart[] { new ProductInCart(ProductId, Quantity) },
                x => x.ProductId
            ).AssertEquals(Bridge.GetShoppingCartItems());
        }
    }
}
