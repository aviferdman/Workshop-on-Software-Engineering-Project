using System.Collections.Generic;

using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public class UseCase_RemoveProductFromCart_TestLogic : MarketTestBase
    {
        public UseCase_RemoveProductFromCart_TestLogic
        (
            SystemContext systemContext,
            IEnumerable<ProductIdentifiable> productsRemove,
            IEnumerable<ProductForCart> productsForCart
        ) : base(systemContext)
        {
            ProductsRemove = productsRemove;
            ProductsForCart = productsForCart;
        }

        public IEnumerable<ProductIdentifiable> ProductsRemove { get; }
        public IEnumerable<ProductForCart> ProductsForCart { get; }

        public void Success_Normal_NoCheckCartItems()
        {
            foreach (ProductIdentifiable product in ProductsRemove)
            {
                Assert.IsTrue(Bridge.RemoveProductFromUserCart(product.ProductId));
            }
        }

        public void Success_Normal_CheckCartItems()
        {
            Success_Normal_NoCheckCartItems();
            CheckCartItems();
        }

        private void CheckCartItems()
        {
            new Assert_SetEquals<ProductInCart>(CalculateExpected())
                .AssertEquals(Bridge.GetShoppingCartItems());
        }

        private IEnumerable<ProductInCart> CalculateExpected()
        {
            IDictionary<ProductId, ProductForCart> expected = ProductForCart.ToDictionary(ProductsForCart);
            foreach (ProductIdentifiable productRemoved in ProductsRemove)
            {
                _ = expected.Remove(productRemoved.ProductId);
            }
            return ProductForCart.ToProductInCart(expected.Values);
        }
    }
}
