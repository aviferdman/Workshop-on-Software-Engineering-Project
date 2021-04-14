using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    /// <summary>
    /// Acceptance test for
    /// Use case 8: remove product from shopping cart
    /// https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/68
    /// </summary>
    [TestFixtureSource(nameof(FixtureArgs))]
    public class UseCase_RemoveProductFromCart : MarketMemberTestBase
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
                30
            },
        };

        public UseCase_RemoveProductFromCart(
            SystemContext systemContext,
            UserInfo buyerUser,
            UserInfo shopOwnerUser,
            ShopInfo shopInfo,
            ProductInfo productInfo,
            int addToCartQuantity
        ) : base(systemContext, buyerUser)
        {
            ShopOwnerUser = shopOwnerUser;
            ShopInfo = shopInfo;
            ProductInfo = productInfo;
            AddToCartQuantity = addToCartQuantity;
        }

        public UserInfo ShopOwnerUser { get; }
        public ShopInfo ShopInfo { get; }
        public ProductInfo ProductInfo { get; }
        public int AddToCartQuantity { get; }

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
                AddToCartQuantity
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
            Assert.IsTrue(Bridge.RemoveProductFromUserCart(useCase_addProductToCart.ProductId));
            new Assert_SetEquals<ProductInCart>("Remove product from cart", Enumerable.Empty<ProductInCart>())
                .AssertEquals(Bridge.GetShoppingCartItems());
        }
    }
}
