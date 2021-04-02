using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.MarketTests
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
                new UserInfo(USER_BUYER_NAME, USER_BUYER_PASSWORD),
                new UserInfo(USER_SHOP_OWNER_NAME, USER_SHOP_OWNER_PASSWORD),
                SHOP_NAME,
                new ProductInfo("speakers", 30, 90),
            },
        };

        public UseCase_RemoveProductFromCart(
            SystemContext systemContext,
            UserInfo buyer_user,
            UserInfo shop_owner_user,
            string shopName,
            ProductInfo productInfo
        ) : base(systemContext, buyer_user)
        {
            Shop_Owner_User = shop_owner_user;
            ShopName = shopName;
            ProductInfo = productInfo;
        }

        public UserInfo Shop_Owner_User { get; }
        public string ShopName { get; }
        public ProductInfo ProductInfo { get; }

        private UseCase_AddProductToCart useCase_addProductToCart;

        public override void Setup()
        {
            base.Setup();

            useCase_addProductToCart = new UseCase_AddProductToCart(
                SystemContext,
                UserInfo,
                Shop_Owner_User,
                ShopName,
                ProductInfo
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
            Assert.IsTrue(Bridge.RemoveProductFromUserCart(useCase_addProductToCart.Product));
            new Assert_SetEquals<ProductId>("Remove product from cart", Enumerable.Empty<ProductId>())
                .AssertEquals(Bridge.GetShoppingCartItems());
        }
    }
}
