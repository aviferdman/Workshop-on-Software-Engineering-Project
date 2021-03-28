using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge) { }

        public override IMarketBridge Bridge => this;

        public Shop? OpenShop(ShopInfo shop)
        {
            return RealBridge?.OpenShop(shop);
        }

        public Product? AddProduct(Shop shop, ProductInfo product)
        {
            return RealBridge?.AddProduct(shop, product);
        }

        public bool RemoveProduct(Shop shop, Product product)
        {
            return RealBridge == null ? false : RealBridge.RemoveProduct(shop, product);
        }
    }
}
