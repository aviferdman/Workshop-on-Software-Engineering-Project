
using System;
using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.Tests.Market.Shop.Products;
using AcceptanceTests.Tests.User;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public class UseCase_AddProductToCart_TestLogic : MarketTestBase
    {
        public UseCase_AddProductToCart_TestLogic(SystemContext systemContext) :
            this(systemContext, null)
        { }
        public UseCase_AddProductToCart_TestLogic(SystemContext systemContext, UserInfo? userInfo) :
            base(systemContext)
        {
            UserInfo = userInfo;
        }

        public UserInfo? UserInfo { get; }
        public List<ProductId>? ProductsTeardown { get; private set; }

        private UseCase_Login useCase_login_buyer;

        public void Setup()
        {
            if (UserInfo == null)
            {
                throw new InvalidOperationException("A user which to add the products to his cart must be specified.");
            }

            new UseCase_LogOut_TestLogic(SystemContext).Success_Normal();
            useCase_login_buyer = new UseCase_Login(SystemContext, UserInfo);
            useCase_login_buyer.Setup();
            useCase_login_buyer.Success_Normal();

            ProductsTeardown = new List<ProductId>();
        }

        public void Teardown()
        {
            if (UserInfo == null)
            {
                throw new InvalidOperationException("A user which to add the products to his cart must be specified.");
            }

            _ = UserBridge.AssureLogin(UserInfo);
            foreach (ProductId productId in ProductsTeardown!)
            {
                _ = MarketBridge.RemoveProductFromUserCart(productId);
            }
            useCase_login_buyer?.Teardown();
        }

        public void Success_Normal_NoCheckCartItems(IEnumerable<ProductInCart> productsAdd)
        {
            foreach (ProductInCart product in productsAdd)
            {
                Assert.IsTrue(MarketBridge.AddProductToUserCart(product));
            }

            ProductsTeardown?.AddRange(productsAdd.Select(x => x.ProductId));
        }

        public void Success_Normal_CheckCartItems(IEnumerable<ProductInCart> productsAdd, IEnumerable<ProductInCart> expected)
        {
            Success_Normal_NoCheckCartItems(productsAdd);
            new Assert_SetEquals<ProductId, ProductInCart>
            (
                expected,
                x => x.ProductId
            ).AssertEquals(MarketBridge.GetShoppingCartItems());
        }
    }
}
