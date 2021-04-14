
using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 9: change quantity of product in shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/69
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_EditProductInCart : MarketMemberTestBase
    {
        static object[] FixtureArgs =
        {
            new object[]
            {
                SystemContext.Instance,
                User_Buyer,
                User_ShopOwner1,
                Shop1,
                new ProductInfo("speakers", 30, 90),
                10,
                20,
            },
        };

        public UseCase_EditProductInCart(
            SystemContext systemContext,
            UserInfo buyerUser,
            UserInfo shopOwnerUser,
            ShopInfo shopInfo,
            ProductInfo productInfo,
            int addToCartQuantity,
            int newQuantity
        ) : base(systemContext, buyerUser)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopInfo = shopInfo;
            ProductInfo = productInfo;
            PrevQuantity = addToCartQuantity;
            NewQuantity = newQuantity;
        }

        public UserInfo ShopOwnerUser { get; }
        public ShopInfo ShopInfo { get; }
        public ProductInfo ProductInfo { get; }
        public int PrevQuantity { get; }
        public int NewQuantity { get; }

        private UseCase_AddProductToCart useCase_addProductToCart;

        public override void Setup()
        {
            base.Setup();

            useCase_addProductToCart = new UseCase_AddProductToCart(
                SystemContext,
                UserInfo,
                ShopOwnerUser,
                ShopInfo,
                ProductInfo,
                PrevQuantity
            );
            useCase_addProductToCart.Setup();
            useCase_addProductToCart.Success_Normal();
        }

        public override void Teardown()
        {
            base.Teardown();
            useCase_addProductToCart.Teardown();
        }

        [TestCase]
        public void Success_Normal()
        {
            Assert.IsTrue(Bridge.EditProductInUserCart(useCase_addProductToCart.ProductId, NewQuantity));
            new Assert_SetEquals<ProductId, ProductInCart>(
                "Edit product in cart - success",
                new ProductInCart[] { new ProductInCart(useCase_addProductToCart.ProductId, NewQuantity) },
                x => x.ProductId
            ).AssertEquals(Bridge.GetShoppingCartItems());
        }

        [TestCase]
        public void Failure_InvalidQuantity()
        {
            Assert.IsFalse(Bridge.EditProductInUserCart(useCase_addProductToCart.ProductId, -1));
            new Assert_SetEquals<ProductId, ProductInCart>(
                "Edit product in cart - invalid quantity",
                new ProductInCart[] { new ProductInCart(useCase_addProductToCart.ProductId, PrevQuantity) },
                x => x.ProductId
            ).AssertEquals(Bridge.GetShoppingCartItems());
        }
    }
}
