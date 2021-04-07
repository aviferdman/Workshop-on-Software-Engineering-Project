using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        /// <summary>
        /// Makes sure the sgop with the specified shop is open and returns the shop id.
        /// Might send a request to create a new shop.
        /// Since shops cannot be deleted, we take similar measures to
        /// <seealso cref="UserBridge.IUserBridge.AssureSignUp(UserInfo)"/>
        /// </summary>
        /// <returns>The opened shop with its id, null if failed.</returns>
        ShopId? AssureOpenShop(ShopInfo shopInfo);

        /// <summary>
        /// Adds the product to the shop and returns the product id.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productInfo"></param>
        /// <returns>The product with its id, null if failed.</returns>
        ProductId? AddProductToShop(ShopId shopId, ProductInfo productInfo);

        bool RemoveProductFromShop(ShopId shopId, ProductId productId);

        bool EditProductInShop(ShopId shopId, ProductId productId, ProductInfo newProductDetails);

        // TODO: add with quantity
        bool AddProductToUserCart(ProductId productId);
        bool RemoveProductFromUserCart(ProductId productId);
        bool EditProductInUserCart(ProductId productId, int quantity);
        bool EditUserCart(ISet<ProductInCart> productsAdd, ISet<ProductId> productsRemove, ISet<ProductInCart> productsEdit);

        // TODO: results should contain the info as well (with the quantity too)
        IEnumerable<ProductId>? GetShoppingCartItems();

        // TODO: results should contain the info as well
        ProductSearchResults? SearchProducts(ProductSearchCreteria creteria);

        ShopInfo? GetShopDetails(ShopId shopId);

        // TODO: results should contain the info too
        // TODO: also add checks comparing the details in the add and edit product
        IEnumerable<ProductId>? GetShopProducts(ShopId shopId);
    }
}
