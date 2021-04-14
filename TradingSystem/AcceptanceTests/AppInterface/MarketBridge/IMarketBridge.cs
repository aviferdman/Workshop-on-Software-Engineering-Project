using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        ProductSearchResults? SearchProducts(ProductSearchCreteria creteria);

        /// <summary>
        /// Tries to open the shop with the specified info.
        /// </summary>
        /// <returns>The opened shop id, null if failed.</returns>
        ShopId? OpenShop(ShopInfo shopInfo);

        /// <summary>
        /// Makes sure the shop with the specified shop is open and returns the shop id.
        /// Might send a request to create a new shop.
        /// Since shops cannot be deleted, we take similar measures to
        /// <seealso cref="UserBridge.IUserBridge.AssureSignUp(UserInfo)"/>
        /// </summary>
        /// <returns>The opened shop id, null if failed.</returns>
        ShopId? AssureOpenShop(ShopInfo shopInfo);

        ShopInfo? GetShopDetails(ShopId shopId);
        IEnumerable<ProductIdentifiable>? GetShopProducts(ShopId shopId);

        /// <summary>
        /// Adds the product to the shop and returns the product id.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productInfo"></param>
        /// <returns>The product id, null if failed.</returns>
        ProductId? AddProductToShop(ShopId shopId, ProductInfo productInfo);
        bool RemoveProductFromShop(ShopId shopId, ProductId productId);
        bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails);
        IEnumerable<ProductInCart>? GetShoppingCartItems();

        bool AddProductToUserCart(ProductInCart product);
        bool RemoveProductFromUserCart(ProductId productId);
        bool EditProductInUserCart(ProductId productId, int quantity);
        bool EditUserCart(ISet<ProductInCart> productsAdd, ISet<ProductId> productsRemove, ISet<ProductInCart> productsEdit);
    }
}
