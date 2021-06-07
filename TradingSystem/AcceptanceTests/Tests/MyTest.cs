using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;
using AcceptanceTests.AppInterface.MarketBridge;
using AcceptanceTests.AppInterface.UserBridge;

using NUnit.Framework;

namespace AcceptanceTests.Tests
{
    public class MyTest
    {
        [TestCase]
        public void Test()
        {
            List<ProductInCart>? cartProducts;
            UserInfo user_buyer = SharedTestsData.User_Buyer;
            UserInfo user_shopOwner = SharedTestsData.User_ShopOwner1;
            ShopInfo shopInfo = SharedTestsData.Shop1;

            IUserBridge bridge_user = SystemContext.Instance.UserBridge;
            IMarketBridge bridge_market = SystemContext.Instance.MarketBridge;

            bridge_market.SetDbDebugMode(true);

            bridge_user.Connect();
            bridge_user.SignUp(user_shopOwner);
            bridge_user.Login(user_shopOwner);
            ShopId shopId = bridge_market.OpenShop(shopInfo)!.Value;
            ProductId productId1 = bridge_market.AddProductToShop(shopId, new ProductInfo
            (
                name: "design of everyday things",
                quantity: 90,
                price: 30,
                category: "books",
                weight: 18
            ))!.Value;
            ProductId productId2 = bridge_market.AddProductToShop(shopId, new ProductInfo
            (
                name: "numerical analysis",
                quantity: 21,
                price: 12,
                category: "books",
                weight: 19
            ))!.Value!;
            bridge_user.Logout();
            bridge_user.SignUp(user_buyer);
            bridge_user.Login(user_buyer);
            bridge_market.AddProductToUserCart(new ProductInCart(productId1, 10));

            // Problematic lines
            bridge_market.AddProductToUserCart(new ProductInCart(productId2, -1));
            bridge_market.AddProductToUserCart(new ProductInCart(productId2, 0));

            cartProducts = bridge_market.GetShoppingCartItems().ToList();
            Assert.IsTrue(cartProducts.Count == 1);

            // Problematic lines
            bridge_market.RemoveProductFromUserCart(productId2);
            bridge_market.RemoveProductFromUserCart(productId2);

            bridge_market.RemoveProductFromUserCart(productId1);
            cartProducts = bridge_market.GetShoppingCartItems().ToList();
            Assert.IsFalse(cartProducts.Any());
            bridge_user.Logout();

            //bridge_user.Login(user_shopOwner);
            //bridge_market.RemoveProductFromShop(shopId, productId1);
            //bridge_market.RemoveProductFromShop(shopId, productId2);
            //bridge_user.Logout();

            bridge_user.Login(user_buyer);
            cartProducts = bridge_market.GetShoppingCartItems().ToList();
            Assert.IsFalse(cartProducts.Any());

            //bridge_market.AddProductToUserCart(new ProductInCart(productId1, 10));
            //Assert.IsTrue(bridge_market.GetShoppingCartItems().Count() == 1);
            //bridge_market.RemoveProductFromUserCart(productId1);
            //Assert.IsTrue(bridge_market.GetShoppingCartItems().Count() == 1);
            //bridge_user.Logout();
            //bridge_user.Login(user_buyer);
            //Assert.IsTrue(!bridge_market.GetShoppingCartItems().Any());
        }
    }
}
