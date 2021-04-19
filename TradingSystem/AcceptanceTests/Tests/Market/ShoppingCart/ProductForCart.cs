using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public struct ProductForCart
    {
        public ProductForCart(ProductIdentifiable productIdentifiable, int cartQuantity)
        {
            ProductIdentifiable = productIdentifiable;
            CartQuantity = cartQuantity;
        }

        public ProductIdentifiable ProductIdentifiable { get; }
        public int CartQuantity { get; }

        public static IEnumerable<ProductInCart> ToProductInCart(IEnumerable<ProductForCart> products)
        {
            return products.Select(x => new ProductInCart(x.ProductIdentifiable.ProductId, x.CartQuantity));
        }

        public static IDictionary<ProductId, ProductForCart> ToDictionary(IEnumerable<ProductForCart> products)
        {
            return products.ToDictionary(x => x.ProductIdentifiable.ProductId);
        }

        public static IDictionary<ProductId, ProductInCart> ToDictionary_InCart(IEnumerable<ProductForCart> products)
        {
            return ToProductInCart(products).ToDictionary(x => x.ProductId);
        }
    }
}
