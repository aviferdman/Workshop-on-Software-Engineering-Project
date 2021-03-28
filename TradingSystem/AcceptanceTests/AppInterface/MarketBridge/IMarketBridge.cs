using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        /// <summary>
        /// Makes sure the sgop with the specified shop is open and returns new shop with its id.
        /// Might send a request to create a new shop.
        /// Since shops cannot be deleted, we take similar measures to
        /// <seealso cref="UserBridge.IUserBridge.AssureSignUp(UserInfo)"/>
        /// </summary>
        /// <returns>The opened shop with its id, null if failed.</returns>
        Shop? AssureOpenShop(ShopInfo shop);

        /// <summary>
        /// Adds the product to the shop and returns the product with its id.
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="product"></param>
        /// <returns>The product with its id, null if failed.</returns>
        Product? AddProduct(Shop shop, ProductInfo product);

        bool RemoveProduct(Shop shop, Product product);

        IEnumerable<Product>? SearchProducts(ProductSearchCreteria creteria);
    }
}
