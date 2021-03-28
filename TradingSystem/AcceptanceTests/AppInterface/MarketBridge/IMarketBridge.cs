using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        /// <summary>
        /// Opens a shop and returns the new shop with its id.
        /// </summary>
        /// <returns>The opened shop with its id, null if failed.</returns>
        Shop? OpenShop(ShopInfo shop);

        /// <summary>
        /// Adds the product to the shop and returns the product with its id.
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="product"></param>
        /// <returns>The product with its id, null if failed.</returns>
        Product? AddProduct(Shop shop, ProductInfo product);

        bool RemoveProduct(Shop shop, Product product);
    }
}
