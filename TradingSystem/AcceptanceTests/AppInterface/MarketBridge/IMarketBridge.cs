namespace AcceptanceTests.AppInterface.MarketBridge
{
    public interface IMarketBridge
    {
        /// <summary>
        /// Opens a shop and returns the new shop id.
        /// </summary>
        /// <returns>The opened shop id. Negative if failed.</returns>
        int OpenShop(string shopName);
    }
}
