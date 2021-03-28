using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.MarketBridge
{
    public class MarketBridgeProxy : ProxyBase<IMarketBridge>, IMarketBridge
    {
        private Dictionary<ShopInfo, Shop> shops;

        public MarketBridgeProxy(IMarketBridge? realBridge) : base(realBridge) { }

        public override IMarketBridge Bridge => this;

        public Shop? AssureOpenShop(ShopInfo shopInfo)
        {
            if (RealBridge == null)
            {
                return null;
            }

            Shop? shop;
            bool exists;
            lock (shops)
            {
                exists = shops.TryGetValue(shopInfo, out shop);
            }
            if (exists)
            {
                if (!shop!.Owner.Equals(SystemContext!.LoggedInUser))
                {
                    throw new ShopOwnerMismatchException(shopInfo);
                }

                return shop;
            }

            shop = RealBridge.AssureOpenShop(shopInfo);
            if (shop != null)
            {
                lock (shops)
                {
                    // Shop class might override Equals and GetHashCode
                    shops.Add(new ShopInfo(shop.Name), shop);
                }
            }
            return shop;
        }

        public Product? AddProduct(Shop shop, ProductInfo productInfo)
        {
            return RealBridge?.AddProduct(shop, productInfo);
        }

        public bool RemoveProduct(Shop shop, Product product)
        {
            return RealBridge != null && RealBridge.RemoveProduct(shop, product);
        }

        public ProductSearchResults? SearchProducts(ProductSearchCreteria creteria)
        {
            return RealBridge?.SearchProducts(creteria);
        }
    }
}
